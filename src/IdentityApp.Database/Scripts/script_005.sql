CREATE TABLE identity.userclaims (
    userid integer NOT NULL REFERENCES identity.users (Id),
    type character varying(255) NOT NULL,
    value character varying(255) NOT NULL
);

ALTER TABLE ONLY identity.userclaims
    ADD CONSTRAINT userclaims_pkey PRIMARY KEY (userid, type);