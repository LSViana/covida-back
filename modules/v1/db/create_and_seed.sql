drop table if exists message;
drop table if exists help_item;
drop table if exists help_has_category;
drop table if exists help;
drop table if exists help_category;
drop table if exists "user";
drop table if exists message;
drop type if exists help_status;
drop type if exists message_status;
-- Creating schema

create table "user"
(
    id            serial primary key,
    name          varchar(64)  not null,
    lat           float        not null,
    lng           float        not null,
    address       varchar(128) not null,
    "isVolunteer" bool         not null,
    "accessToken" varchar(32)  not null
);

create type help_status as enum ('awaiting', 'active', 'past', 'cancelled');

create table help_category
(
    id   varchar(64) primary key,
    name varchar(64) not null
);

create table help
(
    id                  serial primary key,
    datetime            timestamp with time zone not null,
    "cancelledDateTime" bool                     null,
    "cancelledReason"   varchar(64)              null,
    status              help_status              not null,
    "userId"            int                      not null,
    foreign key ("userId") references "user" (id)
);

create table help_has_category
(
    "helpId"         int         not null,
    "helpCategoryId" varchar(64) not null,
    foreign key ("helpId") references help (id),
    foreign key ("helpCategoryId") references help_category (id),
    primary key ("helpId", "helpCategoryId")
);

create table help_item
(
    id       serial primary key,
    name     varchar(64) not null,
    amount   int         not null,
    complete bool        not null,
    "helpId" int         not null,
    foreign key ("helpId") references help (id)
);

create type message_status as enum ('sent', 'read');

create table message
(
    id       serial primary key,
    datetime timestamp with time zone not null,
    text     varchar(64)              not null,
    status   message_status           not null,
    "helpId" int                      not null,
    "userId" int                      not null,
    foreign key ("helpId") references help (id),
    foreign key ("userId") references "user" (id)
);

-- Inserting data
insert into "user"
values (default, 'Lucas Viana', 23, 47, 'Al. Barão de Limeira, 539', true, 'b8d17fda7cf3211fa3b9f3a47cf4c9e9'),
       (default, 'Gustavo Henrique', 24, 46, 'R. Virgílio Malta, 11-22', true, '');

insert into help_category
values ('market', 'Mercado'),
       ('bakery', 'Padaria'),
       ('pharmacy', 'Farmácia'),
       ('other', 'Outros');
