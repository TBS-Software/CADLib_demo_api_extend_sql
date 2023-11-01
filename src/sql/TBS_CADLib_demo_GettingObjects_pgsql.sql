/*
Командный запрос на получение перечня файлов публикаций для коллизий
Сперва создается временная таблица temp_collisions, куда заносится информация по коллизиям в части параметров COLLISION_OBJECT1 и COLLISION_OBJECT2
*/

drop table if exists temp_collisions;
with 
	idobjectcategory_collisions as (select idobjectcategory FROM objectcategories WHERE name = 'COLLISIONS' ORDER BY idobjectcategory ASC LIMIT 1),
	paramdef_object1 as (select idparamdef from paramdefs where name = 'COLLISION_OBJECT1' ORDER BY idparamdef ASC LIMIT 1),
	paramdef_object2 as (select idparamdef from paramdefs where name = 'COLLISION_OBJECT2' ORDER BY idparamdef ASC LIMIT 1),
	idobjects as (select idobject from objectsshadow where idobjectcategory = (select idobjectcategory from idobjectcategory_collisions))
	select idparamdef, idobject, value into temporary table temp_collisions 
		from parameters_str where idobject = any(select idobject from idobjects) and idparamdef in (select unnest(
			array[
				(select idparamdef from paramdef_object1),
				(select idparamdef from paramdef_object2)])) order by idobject;
/*
Создадим временную таблицу temp_collisions_users, куда внесем известную информацию 
для всех объектов по их UID из таблицы temp_collisions. 
*/
drop table if exists temp_collisions_objects;
create table temp_collisions_objects as select idobject, uid from objectsshadow  
	where uid = any(select distinct uuid(value) from temp_collisions);
--Параметр PROJECT_CHECKIN_FILE_NAME
alter table temp_collisions_objects add column publication_name TEXT;
WITH
	paramdef_publ_name as (select idparamdef from paramdefs where name = 'PROJECT_CHECKIN_FILE_NAME' ORDER BY idparamdef ASC LIMIT 1)
	update temp_collisions_objects table1 set publication_name = table2.value
		from parameters_str table2 where table2.idobject = table1.idobject and table2.idparamdef = (select idparamdef from paramdef_publ_name);
--Удаляем прочие таблицы
drop table temp_collisions;
COPY (select distinct publication_name from temp_collisions_objects where publication_name is not null order by publication_name) 
	TO 'C:\Windows\Temp\cadlib_output.txt' DELIMITER '|' CSV HEADER ;
drop table temp_collisions_objects;