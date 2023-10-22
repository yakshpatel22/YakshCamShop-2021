

CREATE  PROCEDURE pGetThreeClosestStores (@LatUser float, @LngUser float)
AS
DECLARE @BranchID INT;
DECLARE @Lat FLOAT;
DECLARE @Lng FLOAT;
DECLARE @Distance FLOAT;
 
DECLARE CUR CURSOR FAST_FORWARD FOR
   SELECT Id FROM Branches
 
OPEN CUR
FETCH NEXT FROM CUR INTO @BranchID
  
WHILE @@FETCH_STATUS = 0
BEGIN
   SELECT @Lat = Latitude, @Lng = Longitude 
      FROM Branches WHERE Id = @BranchID
 
   SET @Distance = SQRT(POWER(@Lat - @LatUser, 2) + POWER(@Lng - @LngUser, 2)) * 1.609344 --Killometers
   UPDATE Branches 
   SET Distance = @Distance
   WHERE Id = @BranchID
 
   FETCH NEXT FROM CUR INTO @BranchID
END
 
CLOSE CUR
DEALLOCATE CUR

SELECT TOP 3 * FROM Branches s 
ORDER BY Distance;