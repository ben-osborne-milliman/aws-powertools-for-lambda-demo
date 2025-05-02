DO
$$
    BEGIN
        IF EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'rds_iam') THEN
            grant rds_iam to lambda_user;
        END IF;
    END
$$;
