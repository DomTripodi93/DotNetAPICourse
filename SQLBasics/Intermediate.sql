
ORDER BY
JOIN
INNER
LEFT
RIGHT
OUTER APPLY & CROSS APPLY
Aggregates & GROUP BY
SUM 
MIN / MAX
STRING_AGG
Nested Queries


USE TestDatabase;

INSERT INTO TestAppSchema.Computer (Motherboard
                                    , CPUCores
                                    , HasWifi
                                    , HasLTE
                                    , ReleaseDate
                                    , Price
                                    , VideoCard)
SELECT  Computer.Motherboard
        , Computer.CPUCores
        , Computer.HasWifi
        , Computer.HasLTE
        , DATEADD(DAY, -10, Computer.ReleaseDate)
        , Computer.Price * 2
        , Computer.VideoCard
  FROM  TestAppSchema.Computer;

SELECT  Motherboard
        , CPUCores
        , HasWifi
        , HasLTE
        , ReleaseDate
        , Price
        , VideoCard
  FROM  TestAppSchema.Computer
 ORDER BY
    Computer.HasWifi
    , Computer.HasLTE DESC
    , Computer.ReleaseDate DESC;



SELECT  TOP 10
        Motherboard
        , Price
    INTO TestAppSchema.ComputerPrice
  FROM  TestAppSchema.Computer
 ORDER BY
    Computer.HasWifi
    , Computer.HasLTE DESC
    , Computer.ReleaseDate DESC;
