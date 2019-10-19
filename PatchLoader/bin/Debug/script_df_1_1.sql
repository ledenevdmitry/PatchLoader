DECLARE
no_table exception;
pragma exception_init(no_table,-942);
BEGIN
  execute immediate 'DROP TABLE op_user.NA2PFRATE002_BRIDGE_BKP';
EXCEPTION
  when no_table then null;
END;
/

create table op_user.NA2PFRATE002_BRIDGE_BKP tablespace trash parallel 8
as
select NK, NA2EAN, NA2RAT, NA2DB, NA2DE, YAROED, YARCED, DWSAUTO, DWSARCHIVE, DWSJOB from dws002_eq.NA2PFRATE002_BRIDGE_MIRROR where 1=0;

alter table op_user.NA2PFRATE002_BRIDGE_BKP add (na2ean_new varchar2(20), NA2DE_new number);

insert into op_user.NA2PFRATE002_BRIDGE_BKP
select
m.NK, m.NA2EAN, m.NA2RAT, m.NA2DB, m.NA2DE, m.YAROED, m.YARCED, m.DWSAUTO, m.DWSARCHIVE, m.DWSJOB,
n.na2ean as na2ean_new, n.NA2DE as NA2DE_new
from
dws002_eq.NA2PFRATE002_BRIDGE_MIRROR m left join
op_user.NA2PFRATE002_BRIDGE_NEW n on n.na2ean = m.na2ean and m.NA2DB = n.NA2DB;

commit;
/