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
-- Data for Name: product; Type: TABLE DATA; Schema: products; Owner: -
--

INSERT INTO products.product (id, name, price, category) VALUES (1, 'Kayak', 275.00, 'Watersports');
INSERT INTO products.product (id, name, price, category) VALUES (2, 'Lifejacket', 48.95, 'Watersports');
INSERT INTO products.product (id, name, price, category) VALUES (3, 'Soccer Ball', 19.50, 'Soccer');
INSERT INTO products.product (id, name, price, category) VALUES (4, 'Corner Flags', 34.95, 'Soccer');
INSERT INTO products.product (id, name, price, category) VALUES (5, 'Stadium', 79500.00, 'Soccer');
INSERT INTO products.product (id, name, price, category) VALUES (6, 'Thinking Cap', 16.00, 'Chess');
INSERT INTO products.product (id, name, price, category) VALUES (7, 'Unsteady Chair', 29.95, 'Chess');
INSERT INTO products.product (id, name, price, category) VALUES (8, 'Human Chess Board', 75.00, 'Chess');
INSERT INTO products.product (id, name, price, category) VALUES (9, 'Bling-Bling King', 1200.00, 'Chess');


--
-- Name: product_id_seq; Type: SEQUENCE SET; Schema: products; Owner: -
--

SELECT pg_catalog.setval('products.product_id_seq', 11, true);


--
-- PostgreSQL database dump complete
--

