USE brewery
GO

INSERT INTO Breweries (Name)
VALUES ('Piedboeuf')

INSERT INTO Beers (Name, Price, AlcoolPercentage, BreweryId)
VALUES ('Jupiler', 0.57, 5.2, SCOPE_IDENTITY())
DECLARE @JupBeerId INT = SCOPE_IDENTITY()

INSERT INTO Breweries (Name)
VALUES ('Brouwerij Maes')
DECLARE @MaesBierId INT = SCOPE_IDENTITY()

INSERT INTO Beers (Name, Price, AlcoolPercentage, BreweryId)
VALUES ('Maes', 0.55, 5.2, SCOPE_IDENTITY())


INSERT INTO Wholesalers (Name) VALUES ('Bierschuur')

INSERT INTO WholesalerStocks (BeerId, WholesalerId, Count)
VALUES (@JupBeerId, SCOPE_IDENTITY(), 24)

INSERT INTO Wholesalers (Name) VALUES ('Epicerie du coin')

INSERT INTO WholesalerStocks (BeerId, WholesalerId, Count)
VALUES (@MaesBierId, SCOPE_IDENTITY(), 205)
