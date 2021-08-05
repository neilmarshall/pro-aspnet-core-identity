--
-- PostgreSQL database dump
--

-- Dumped from database version 11.4
-- Dumped by pg_dump version 11.4

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
--SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: identity; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA identity;


--
-- Name: products; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA products;


SET default_tablespace = '';

SET default_with_oids = false;

--
-- Name: users; Type: TABLE; Schema: identity; Owner: -
--

CREATE TABLE identity.users (
    username character varying(255),
    normalizedusername character varying(255),
    email character varying(255),
    normalizedemail character varying(255),
    emailconfirmed boolean,
    passwordhash character varying(255),
    securitystamp character varying(255),
    concurrencystamp character varying(255),
    accessfailedcount integer,
    id integer NOT NULL
);


--
-- Name: users_id_seq; Type: SEQUENCE; Schema: identity; Owner: -
--

CREATE SEQUENCE identity.users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: users_id_seq; Type: SEQUENCE OWNED BY; Schema: identity; Owner: -
--

ALTER SEQUENCE identity.users_id_seq OWNED BY identity.users.id;


--
-- Name: product; Type: TABLE; Schema: products; Owner: -
--

CREATE TABLE products.product (
    id integer NOT NULL,
    name character varying(255),
    price numeric(8,2),
    category character varying(255)
);


--
-- Name: product_id_seq; Type: SEQUENCE; Schema: products; Owner: -
--

CREATE SEQUENCE products.product_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: product_id_seq; Type: SEQUENCE OWNED BY; Schema: products; Owner: -
--

ALTER SEQUENCE products.product_id_seq OWNED BY products.product.id;


--
-- Name: users id; Type: DEFAULT; Schema: identity; Owner: -
--

ALTER TABLE ONLY identity.users ALTER COLUMN id SET DEFAULT nextval('identity.users_id_seq'::regclass);


--
-- Name: product id; Type: DEFAULT; Schema: products; Owner: -
--

ALTER TABLE ONLY products.product ALTER COLUMN id SET DEFAULT nextval('products.product_id_seq'::regclass);


--
-- Name: users users_pkey; Type: CONSTRAINT; Schema: identity; Owner: -
--

ALTER TABLE ONLY identity.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);


--
-- Name: product product_pkey; Type: CONSTRAINT; Schema: products; Owner: -
--

ALTER TABLE ONLY products.product
    ADD CONSTRAINT product_pkey PRIMARY KEY (id);


--
-- PostgreSQL database dump complete
--

