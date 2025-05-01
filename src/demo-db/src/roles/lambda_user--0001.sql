create user lambda_user;

DO
$$
    BEGIN
        IF EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'rds_iam') THEN
            grant rds_iam to api_user;
        END IF;
    END
$$;

grant usage on schema ecommerce to lambda_user;
grant all privileges on all tables in schema ecommerce to lambda_user;
grant all privileges on all sequences in schema ecommerce to lambda_user;

alter default privileges in schema ecommerce grant all privileges on tables to lambda_user;
alter default privileges in schema ecommerce grant all privileges on sequences to lambda_user;