create table Discount(
	id serial primary key,
	user_id varchar(100) not null,
	rate smallint not null,
	code varchar(30) not null,
	created_date timestamp not null default CURRENT_TIMESTAMP
)