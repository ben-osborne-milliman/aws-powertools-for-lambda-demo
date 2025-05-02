create user lambda_user;

grant usage on schema ecommerce to lambda_user;
grant all privileges on all tables in schema ecommerce to lambda_user;
grant all privileges on all sequences in schema ecommerce to lambda_user;

alter default privileges in schema ecommerce grant all privileges on tables to lambda_user;
alter default privileges in schema ecommerce grant all privileges on sequences to lambda_user;