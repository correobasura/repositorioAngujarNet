Admin198853406$

DROP USER userPrueba CASCADE;
CREATE USER userPrueba IDENTIFIED BY userPrueba;
GRANT RESOURCE TO userPrueba;
GRANT CREATE SESSION TO userPrueba;

SELECT s.sid, s.serial#, s.status, p.spid 
FROM v$session s, v$process p 
WHERE s.username = 'USERPRUEBA'
AND p.addr(+) = s.paddr;

ALTER SYSTEM KILL SESSION '141,91';

DROP USER userIdentity CASCADE;
CREATE USER userIdentity IDENTIFIED BY userIdentity;
GRANT RESOURCE TO userIdentity;
GRANT CREATE SESSION TO userIdentity;
USERIDENTITY

DROP USER userPrueba CASCADE;