create table transactions (
	t_id int auto_increment primary key not null,
    origin varchar(100),
    req_type varchar(10),
    t_time datetime default current_timestamp,
    t_type varchar(100),
    t_account varchar(100) default "",
    t_price float default -1.0,
    t_quantity int default -1,
	username varchar(50) default ""
);

create table BuySell (
	b_id int auto_increment primary key not null,
    b_type varchar(10),
    username varchar(50),
    t_account varchar(100),
	price float check (price > 0),
    quantity int check (quantity > 0)
);

select * from transactions;
select * from BuySell;
