--
-- PostgreSQL database dump
--

-- Dumped from database version 11.4
-- Dumped by pg_dump version 11.4

-- Started on 2021-08-17 22:32:47 BST

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

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 205 (class 1259 OID 31227)
-- Name: roles; Type: TABLE; Schema: identity; Owner: -
--

CREATE TABLE identity.roles (
    id integer NOT NULL,
    name character varying(255) NOT NULL,
    normalizedname character varying(255) NOT NULL,
    concurrencystamp character varying(255)
);


--
-- TOC entry 204 (class 1259 OID 31225)
-- Name: roles_id_seq; Type: SEQUENCE; Schema: identity; Owner: -
--

CREATE SEQUENCE identity.roles_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3157 (class 0 OID 0)
-- Dependencies: 204
-- Name: roles_id_seq; Type: SEQUENCE OWNED BY; Schema: identity; Owner: -
--

ALTER SEQUENCE identity.roles_id_seq OWNED BY identity.roles.id;


--
-- TOC entry 206 (class 1259 OID 31236)
-- Name: userroles; Type: TABLE; Schema: identity; Owner: -
--

CREATE TABLE identity.userroles (
    userid integer NOT NULL,
    roleid integer NOT NULL
);


--
-- TOC entry 3024 (class 2604 OID 31230)
-- Name: roles id; Type: DEFAULT; Schema: identity; Owner: -
--

ALTER TABLE ONLY identity.roles ALTER COLUMN id SET DEFAULT nextval('identity.roles_id_seq'::regclass);


--
-- TOC entry 3026 (class 2606 OID 31235)
-- Name: roles roles_pkey; Type: CONSTRAINT; Schema: identity; Owner: -
--

ALTER TABLE ONLY identity.roles
    ADD CONSTRAINT roles_pkey PRIMARY KEY (id);


--
-- TOC entry 3028 (class 2606 OID 31240)
-- Name: userroles userroles_pkey; Type: CONSTRAINT; Schema: identity; Owner: -
--

ALTER TABLE ONLY identity.userroles
    ADD CONSTRAINT userroles_pkey PRIMARY KEY (userid, roleid);


--
-- TOC entry 3030 (class 2606 OID 31246)
-- Name: userroles userroles_roleid_fkey; Type: FK CONSTRAINT; Schema: identity; Owner: -
--

ALTER TABLE ONLY identity.userroles
    ADD CONSTRAINT userroles_roleid_fkey FOREIGN KEY (roleid) REFERENCES identity.roles(id);


--
-- TOC entry 3029 (class 2606 OID 31241)
-- Name: userroles userroles_userid_fkey; Type: FK CONSTRAINT; Schema: identity; Owner: -
--

ALTER TABLE ONLY identity.userroles
    ADD CONSTRAINT userroles_userid_fkey FOREIGN KEY (userid) REFERENCES identity.users(id);


-- Completed on 2021-08-17 22:32:49 BST

--
-- PostgreSQL database dump complete
--

