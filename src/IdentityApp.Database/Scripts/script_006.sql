CREATE OR REPLACE FUNCTION identity.create_user(
    username character varying(255),
    normalizedusername character varying(255),
    email character varying(255),
    normalizedemail character varying(255),
    emailconfirmed boolean,
    passwordhash character varying(255),
    securitystamp character varying(255),
    concurrencystamp character varying(255),
    accessfailedcount integer)
RETURNS integer
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO identity.users (
        username,
        normalizedusername,
        email,
        normalizedemail,
        emailconfirmed,
        passwordhash,
        securitystamp,
        concurrencystamp,
        accessfailedcount,
        lockoutenabled,
        lockoutend
    ) VALUES (
        username,
        normalizedusername,
        email,
        normalizedemail,
        emailconfirmed,
        passwordhash,
        securitystamp,
        concurrencystamp,
        accessfailedcount,
        true,
        null
    );
    RETURN currval('identity.users_id_seq');
END;
$$;


CREATE OR REPLACE FUNCTION identity.update_user(
    userid integer,
    username character varying(255),
    normalizedusername character varying(255),
    email character varying(255),
    normalizedemail character varying(255),
    emailconfirmed boolean,
    passwordhash character varying(255),
    securitystamp character varying(255),
    concurrencystamp character varying(255),
    accessfailedcount integer,
    lockoutenabled boolean,
    lockoutend timestamp with time zone)
RETURNS void
LANGUAGE plpgsql
AS $$
BEGIN
    UPDATE identity.users SET
        username = update_user.username,
        normalizedusername = update_user.normalizedusername,
        email = update_user.email,
        normalizedemail = update_user.normalizedemail,
        emailconfirmed = update_user.emailconfirmed,
        passwordhash = update_user.passwordhash,
        securitystamp = update_user.securitystamp,
        concurrencystamp = update_user.concurrencystamp,
        accessfailedcount = update_user.accessfailedcount,
        lockoutenabled =  update_user.lockoutenabled,
        lockoutend = update_user.lockoutend
     WHERE id = userid;
END;
$$;


CREATE OR REPLACE FUNCTION identity.delete_user(userid integer)
RETURNS void
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM identity.users WHERE id = userid;
END;
$$;


CREATE OR REPLACE FUNCTION identity.add_user_role(userid integer, rolename character varying(255))
RETURNS void
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO identity.userroles SELECT userid, id FROM identity.roles WHERE normalizedname = UPPER(rolename);
END;
$$;


CREATE OR REPLACE FUNCTION identity.remove_user_role(userid integer, rolename character varying(255))
RETURNS void
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM identity.userroles
     WHERE identity.userroles.userid = remove_user_role.userid
       AND roleid = (SELECT id FROM identity.roles WHERE normalizedname = rolename);
END;
$$;


CREATE OR REPLACE FUNCTION identity.get_user_roles(userid integer)
RETURNS TABLE (rolename character varying(255))
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT r.name
      FROM identity.roles r
      JOIN identity.userroles ur
        ON r.id = ur.roleid
      JOIN identity.users u
        ON u.id = ur.userid
     WHERE u.id = get_user_roles.userid;
END;
$$;


CREATE OR REPLACE FUNCTION identity.get_users_in_role(rolename character varying(255))
RETURNS SETOF identity.users
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT *
      FROM identity.users
     WHERE id IN (SELECT userid FROM identity.userroles JOIN identity.roles ON roleid = id WHERE normalizedname = UPPER(rolename));
END;
$$;


CREATE OR REPLACE FUNCTION identity.create_role(
    name character varying(255),
    normalizedname character varying(255),
    concurrencystamp character varying(255))
RETURNS integer
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO identity.roles (
        name,
        normalizedname,
        concurrencystamp
    ) VALUES (
        name,
        normalizedname,
        concurrencystamp
    );
    RETURN currval('identity.roles_id_seq');
END;
$$;


CREATE OR REPLACE FUNCTION identity.update_role(
    roleid integer,
    name character varying(255),
    normalizedname character varying(255),
    concurrencystamp character varying(255))
RETURNS void
LANGUAGE plpgsql
AS $$
BEGIN
    UPDATE identity.roles SET
        name = update_role.name,
        normalizedname = update_role.normalizedname,
        concurrencystamp = update_role.concurrencystamp
     WHERE id = roleid;
END;
$$;


CREATE OR REPLACE FUNCTION identity.delete_role(roleid integer)
RETURNS void
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM identity.roles WHERE id = roleid;
END;
$$;


CREATE OR REPLACE FUNCTION identity.user_is_in_role(
    userid integer,
    rolename character varying(255))
RETURNS boolean
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN (
    SELECT userid IN (SELECT ur.userid
                        FROM identity.userroles ur
                        JOIN identity.roles r
                          ON r.id = ur.roleid
                       WHERE r.normalizedname = UPPER(rolename))
    );
END;
$$;


CREATE OR REPLACE FUNCTION identity.get_claims(userid integer)
RETURNS TABLE (type character varying(255),  value character varying(255))
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT uc.type, uc.value
      FROM identity.userclaims uc
     WHERE uc.userid = get_claims.userid;
END;
$$;


CREATE OR REPLACE FUNCTION identity.add_claim(
    userid integer,
    type character varying(255),
    value character varying(255)) 
RETURNS VOID
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO identity.userclaims (
        userid,
        type,
        value
    ) VALUES (
        userid,
        type,
        value
    );
END;
$$;


CREATE OR REPLACE FUNCTION identity.delete_claim(
    userid integer,
    type character varying(255)) 
RETURNS VOID
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM identity.userclaims WHERE userclaims.userid = delete_claim.userid AND userclaims.type = delete_claim.type;
END;
$$;


CREATE OR REPLACE FUNCTION identity.update_claim(
    userid integer,
    type character varying(255),
    value character varying(255)) 
RETURNS VOID
LANGUAGE plpgsql
AS $$
BEGIN
    UPDATE identity.userclaims uc
       SET value = update_claim.value
     WHERE uc.userid = update_claim.userid
       AND uc.type = update_claim.type;
END;
$$;