DROP TABLE m2.RateRegionCurrencies

CREATE TABLE m2.RateRegionCurrencies (
ID INT IDENTITY(1,1),
Prefix VARCHAR(20) NOT NULL,
Region VARCHAR(100) NOT NULL,
Multiplier DECIMAL(3,2) NOT NULL
)

INSERT INTO m2.RateRegionCurrencies VALUES ('CAN', 'Canada', 1.1), ('EUR', 'Europe', 1.1), ('AUS', 'Australia', 0.85), ('LA', 'Latin America', 1.1)

ALTER PROCEDURE m2.getRateCurrencyMultipliers
AS

SELECT Prefix, Region, Multiplier FROM m2.RateRegionCurrencies


