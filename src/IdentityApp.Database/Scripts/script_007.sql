CREATE TYPE identity.claim AS (type character varying(255), value character varying(255));

CREATE OR REPLACE FUNCTION identity.add_claim(
    userid integer,
    claims identity.claim[])
RETURNS VOID
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO identity.userclaims (SELECT userid, type, value FROM unnest(claims));
END;
$$;