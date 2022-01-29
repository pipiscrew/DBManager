# Reporting by MySQL with views & procedures

This minimal interface allows user to execute views & procedures. To remember you

### views can be called as 
```sql
select * from table_name;
```   

### procedures called as 
```sql
CALL procname();
--or when has parameters
CALL procname(1,'test',3);
```

the only limitation is when a procedure has an **IN** parameter as varchar, user at dialiog has to **monoquote** the string.  


* Procedures with **OUT** parameters not supported.
* `Download as CSV` supported.