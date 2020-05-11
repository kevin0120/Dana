USE [PROTraveller]
GO
/****** Object:  Trigger [dbo].[AutoUpdateStatus]    Script Date: 2020/5/11 13:35:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER TRIGGER [dbo].[AutoUpdateStatus]
   ON  [dbo].[SN]
   AFTER UPDATE
AS 

if(update(Status))
BEGIN
	SET NOCOUNT ON;
	---In progress
	---Not started
	---Completed

	declare @temp_temp int
     --创建游标 --Local(本地游标)
    DECLARE aaa CURSOR for select PRO_Id from inserted where Status='Completed'
    --打开游标
     Open aaa
     --遍历和获取游标
     fetch next from aaa into @temp_temp
     --print @@fetch_status=0
	 WHILE @@FETCH_STATUS =0
     begin
	 print @temp_temp
	 UPDATE PRO SET  Status='Completed' 
	 where Id=@temp_temp AND 
	 TotalQuantity=(select count(*) from SN where PRO_Id=@temp_temp and Status='Completed')

     fetch next from aaa into @temp_temp --取值赋给变量
     end


     --关闭游标
     Close aaa
     --删除游标
     Deallocate aaa

	  --- alter table PRO add constraint OneOrder unique (OrderNumber)
END
