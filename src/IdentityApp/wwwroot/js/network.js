﻿const baseUrl = "https://localhost:5001/api/data";
const authUrl = "https://localhost:5001/api/auth";

const baseRequestConfig = {
    credentials: "include"
};

export const signIn = async (email, password, callback, errorHandler) => {
    const response = await fetch(`${authUrl}/signin`, {
        ...baseRequestConfig,
        method: "POST",
        body: JSON.stringify({ email, password }),
        headers: {
            "Content-Type": "application/json"
        }
    });

    if (response.ok) {
        let responseData = await response.json();

        if (responseData.success) {
            baseRequestConfig.headers = {
                "Authorization": `Bearer ${responseData.token}`
            }
        }

        processResponse(response, async () => callback(responseData, errorHandler));

        return;
    }

    processResponse({ ok: false, status: "Auth Failed" }, async () => callback(responseData), errorHandler);
};

export const signOut = async (callback) => {
    //const response = await fetch(`${authUrl}/signout`, {
    //    ...baseRequestConfig,
    //    method: "POST"
    //});
    baseRequestConfig.headers = {};

    processResponse({ ok: true }, callback, callback);
};

export const loadData = async (callback, errorHandler) => {
    const response = await fetch(baseUrl, {
        ...baseRequestConfig,
        redirect: "manual"
    });

    processResponse(response, async () => callback(await response.json()), errorHandler);
};

export const createProduct = async (product, callback, errorHandler) => {
    const response = await fetch(baseUrl, {
        ...baseRequestConfig,
        method: "POST",
        body: JSON.stringify(product),
        headers: {
            ...baseRequestConfig.headers,
            "Content-Type": "application/json"
        }
    });

    processResponse(response, callback, errorHandler);
};

export const deleteProduct = async (id, callback, errorHandler) => {
    const response = await fetch(`${baseUrl}/${id}`, {
        ...baseRequestConfig,
        method: "DELETE"
    });

    processResponse(response, callback, errorHandler);
};

function processResponse(response, callback, errorHandler) {
    if (response.ok) {
        callback();
    } else {
        errorHandler(response.status);
    }
}