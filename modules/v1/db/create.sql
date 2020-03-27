create table "user"
(
    id          serial primary key,
    name        varchar(64)  not null,
    lat         float        not null,
    lng         float        not null,
    address     varchar(128) not null,
    isVolunteer bool         not null
);

create type help_status as enum ('awaiting', 'active', 'past', 'cancelled');

create table help
(
    id                serial primary key,
    datetime          timestamp with time zone not null,
    cancelledDateTime bool                     not null,
    cancelledReason   varchar(64)              not null,
    status            help_status               not null,
    userId            int                      not null,
    foreign key (userId) references "user" (id)
);

create table help_item
(
    id       serial primary key,
    name     varchar(64) not null,
    amount   int         not null,
    complete bool        not null,
    helpId   int         not null,
    foreign key (helpId) references help (id)
);

create type message_status as enum ('sent', 'read');

create table message
(
    id       serial primary key,
    datetime timestamp with time zone not null,
    text     varchar(64)              not null,
    status   message_status            not null,
    helpId   int                      not null,
    userId   int                      not null,
    foreign key (helpId) references help (id),
    foreign key (userId) references "user" (id)
);

