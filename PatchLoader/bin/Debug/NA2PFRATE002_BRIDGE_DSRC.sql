ALTER TABLE DWS002_EQ.NA2PFRATE002_BRIDGE_DSRC ADD (DEL_FLAG VARCHAR2(1));

ALTER TABLE DWS002_EQ.NA2PFRATE002_BRIDGE_DSRC RENAME COLUMN DWSSRCDTTM TO DWSTIMESTAMP;

COMMENT ON COLUMN DWS002_EQ.NA2PFRATE002_BRIDGE_DSRC.DEL_FLAG IS '���� ��������';
