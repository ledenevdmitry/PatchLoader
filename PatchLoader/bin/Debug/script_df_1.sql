DECLARE
no_table exception;
pragma exception_init(no_table,-942);
BEGIN
  execute immediate 'DROP TABLE op_user.NA2PFRATE002_BRIDGE_NEW';
EXCEPTION
  when no_table then null;
END;
/

create table op_user.NA2PFRATE002_BRIDGE_NEW tablespace trash parallel 8
as
select na2ean, na2rat, na2db, na2de, yaroed, yarced from dws002_eq.NA2PFRATE002_BRIDGE_MIRROR where 1=0;

insert into op_user.NA2PFRATE002_BRIDGE_NEW
with na2 as (
      select na2ean, na2rat, na2db, na2de
      from dws002_eq.na2pf002_mirror na2m
    )
,
src as (    
select na2ean, na2rat, na2db,
       nvl(lead(na2db) over(partition by na2ean order by na2db), max_de) as na2de,
       yaroed, yarced
from (select na2ean, na2rat, na2db, na2de,
             decode(lag(na2rat) over(partition by na2ean order by na2db), na2rat, 0, 1) ch_flg,
             max(na2de) over(partition by na2ean, na2rat) max_de,
             yaroed, yarced
      from (select na2ean, na2rat, na2db, na2de, yaroed, yarced
            from na2,
                 dws002_eq.yarpfsavings002_bridge_mirror yar
            where na2.na2ean=yar.yarean
                  and nvl(yar.dwsarchive,'N')='N'
           )
     )
where ch_flg=1)
select
na2ean, 
na2rat, 
na2db,
lead(
case when 
                   decode(src.NA2DE,
                   9999999,to_date('59991231','YYYYMMDD'), 
                   0, to_date('59991231','YYYYMMDD'), 
                   case when length(to_char(src.NA2DE)) < 6 then to_date('19000101','YYYYMMDD') else 
                        to_date(to_char(src.NA2DE+ 19000000),'YYYYMMDD') + 1
                   end
                   ) = to_date('59991231','YYYYMMDD') then src.NA2DE
    when 
                   decode(src.NA2DE,
                   9999999,to_date('59991231','YYYYMMDD'), 
                   0, to_date('59991231','YYYYMMDD'), 
                   case when length(to_char(src.NA2DE)) < 6 then to_date('19000101','YYYYMMDD') else 
                        to_date(to_char(src.NA2DE+ 19000000),'YYYYMMDD') + 1
                   end
                   ) = to_date('19000101','YYYYMMDD') then src.NA2DE    
   else 
                  to_number(to_char(to_date(to_char(src.NA2DE+ 19000000),'YYYYMMDD')+1,'YYYYMMDD'))-19000000
   end,1,src.na2de) over (partition by src.na2ean order by src.na2db) as na2de,
yaroed, 
yarced
from
src;

commit;
/