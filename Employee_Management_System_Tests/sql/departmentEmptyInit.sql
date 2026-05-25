TRUNCATE TABLE
    department,
    employee,
    administrator
RESTART IDENTITY CASCADE;

INSERT INTO administrator (user_id, password, user_name) VALUES ('id4321', 'pass', 'hogehoge');
INSERT INTO administrator (user_id, password, user_name) VALUES ('id1234', 'pass2', 'hoge');
INSERT INTO administrator (user_id, password, user_name) VALUES ('dklsajfl34398', 'pass3', 'foofoo');
