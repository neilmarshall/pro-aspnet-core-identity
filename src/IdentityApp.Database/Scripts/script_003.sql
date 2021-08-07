alter table identity.users ADD lockoutenabled boolean not null default false;
alter table identity.users ADD lockoutend timestamp default null;