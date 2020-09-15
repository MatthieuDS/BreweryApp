using BreweryApp.ApiModels;
using BreweryApp.Contexts;
using BreweryApp.Exceptions;
using BreweryApp.Exceptions.RequestQuote;
using BreweryApp.Models;
using BreweryApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BreweryApp.Tests
{
    [TestClass]
    public class TestQuoteService
    {
        [TestMethod]
        [ExpectedException(typeof(EmptyQuoteRequestException))]
        public async Task TestGetQuoteBeersNullShouldThrow()
        {
            var breweryContextMoq = new Mock<IBreweryContext>();
            QuoteService service = new QuoteService(breweryContextMoq.Object);
            RequestQuote quote = new RequestQuote()
            {
                WholesalerId = 1,
                Beers = null
            };

            await service.GetQuote(quote);
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyQuoteRequestException))]
        public async Task TestGetQuoteBeersEmptyShouldThrow()
        {
            var breweryContextMoq = new Mock<IBreweryContext>();
            QuoteService service = new QuoteService(breweryContextMoq.Object);
            RequestQuote quote = new RequestQuote()
            {
                WholesalerId = 1,
                Beers = new List<RequestQuoteBeers>()
            };

            await service.GetQuote(quote);
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyQuoteRequestException))]
        public async Task TestGetQuoteBeersWithoutCountShouldThrow()
        {
            var breweryContextMoq = new Mock<IBreweryContext>();
            QuoteService service = new QuoteService(breweryContextMoq.Object);
            RequestQuote quote = new RequestQuote()
            {
                WholesalerId = 1,
                Beers = new List<RequestQuoteBeers>()
                {
                    new RequestQuoteBeers()
                    {
                        BeerId = 1,
                        Count = 0,
                    }
                }
            };

            await service.GetQuote(quote);
        }

        [TestMethod]
        [ExpectedException(typeof(WholesalerNotFoundException))]
        public async Task TestGetQuoteWholesalerNotFoundShouldThrow()
        {
            var breweryContextMoq = new Mock<IBreweryContext>();
            breweryContextMoq.Setup(bq => bq.GetWholesaler(1))
                .Returns(Task.FromResult<Wholesaler>(null));


            QuoteService service = new QuoteService(breweryContextMoq.Object);
            RequestQuote quote = new RequestQuote()
            {
                WholesalerId = 1,
                Beers = new List<RequestQuoteBeers>()
                {
                    new RequestQuoteBeers()
                    {
                        BeerId = 1,
                        Count = 10
                    }
                }
            };

            await service.GetQuote(quote);
        }


        [TestMethod]
        [ExpectedException(typeof(DoubleBeerRequestedException))]
        public async Task TestGetQuoteDoubleBeerInRequestShouldThrow()
        {
            var breweryContextMoq = new Mock<IBreweryContext>();
            breweryContextMoq.Setup(bq => bq.GetWholesaler(1))
                .Returns(Task.FromResult(new Wholesaler()
                {
                    Id = 1,
                    Name = "TestWholeSaler",
                    WholesalerStocks = new List<WholesalerStock>()
                    {
                        new WholesalerStock()
                        {
                            WholesalerId = 1,
                            BeerId = 25,
                            Beer = new Beer()
                            {
                                Id = 25,
                                Name = "TestBeer",
                                AlcoolPercentage = 99.0M,
                                Price = 0.59M
                            }
                        }
                    }
                }));


            QuoteService service = new QuoteService(breweryContextMoq.Object);
            RequestQuote quote = new RequestQuote()
            {
                WholesalerId = 1,
                Beers = new List<RequestQuoteBeers>()
                {
                    new RequestQuoteBeers()
                    {
                        BeerId = 25,
                        Count = 10
                    },
                    new RequestQuoteBeers()
                    {
                        BeerId = 25,
                        Count = 29
                    },
                }
            };

            await service.GetQuote(quote);
        }

        [TestMethod]
        [ExpectedException(typeof(BeerNotSoldByWholesalerException))]
        public async Task TestGetQuoteRequestBeerWholesalerDoesntSellShouldThrow()
        {
            var breweryContextMoq = new Mock<IBreweryContext>();
            breweryContextMoq.Setup(bq => bq.GetWholesaler(1))
                .Returns(Task.FromResult(new Wholesaler()
                {
                    Id = 1,
                    Name = "TestWholeSaler",
                    WholesalerStocks = new List<WholesalerStock>()
                    {
                        new WholesalerStock()
                        {
                            WholesalerId = 1,
                            BeerId = 25,
                            Count = 200,
                            Beer = new Beer()
                            {
                                Id = 25,
                                Name = "TestBeer",
                                AlcoolPercentage = 99.0M,
                                Price = 0.59M
                            }
                        }
                    }
                }));


            QuoteService service = new QuoteService(breweryContextMoq.Object);
            RequestQuote quote = new RequestQuote()
            {
                WholesalerId = 1,
                Beers = new List<RequestQuoteBeers>()
                {
                    new RequestQuoteBeers()
                    {
                        BeerId = 25,
                        Count = 10
                    },
                    new RequestQuoteBeers()
                    {
                        BeerId = 19,
                        Count = 29
                    },
                }
            };

            await service.GetQuote(quote);
        }


        [TestMethod]
        [ExpectedException(typeof(NotEnoughStockException))]
        public async Task TestGetQuoteRequestBeerNotEnoughInStockShouldThrow()
        {
            var breweryContextMoq = new Mock<IBreweryContext>();
            breweryContextMoq.Setup(bq => bq.GetWholesaler(1))
                .Returns(Task.FromResult(new Wholesaler()
                {
                    Id = 1,
                    Name = "TestWholeSaler",
                    WholesalerStocks = new List<WholesalerStock>()
                    {
                        new WholesalerStock()
                        {
                            WholesalerId = 1,
                            BeerId = 25,
                            Count = 200,
                            Beer = new Beer()
                            {
                                Id = 25,
                                Name = "TestBeer",
                                AlcoolPercentage = 99.0M,
                                Price = 0.59M
                            }
                        }
                    }
                }));


            QuoteService service = new QuoteService(breweryContextMoq.Object);
            RequestQuote quote = new RequestQuote()
            {
                WholesalerId = 1,
                Beers = new List<RequestQuoteBeers>()
                {
                    new RequestQuoteBeers()
                    {
                        BeerId = 25,
                        Count = 201
                    },
                }
            };

            await service.GetQuote(quote);
        }


        [TestMethod]
        public async Task TestGetQuotePriceWithLessThan10Beers()
        {
            var breweryContextMoq = new Mock<IBreweryContext>();
            decimal beerPrice = 0.59M;
            int beerRequested = 9;
            breweryContextMoq.Setup(bq => bq.GetWholesaler(1))
                .Returns(Task.FromResult(new Wholesaler()
                {
                    Id = 1,
                    Name = "TestWholeSaler",
                    WholesalerStocks = new List<WholesalerStock>()
                    {
                        new WholesalerStock()
                        {
                            WholesalerId = 1,
                            BeerId = 25,
                            Count = 200,
                            Beer = new Beer()
                            {
                                Id = 25,
                                Name = "TestBeer",
                                AlcoolPercentage = 99.0M,
                                Price = beerPrice
                            }
                        }
                    }
                }));


            QuoteService service = new QuoteService(breweryContextMoq.Object);
            RequestQuote quote = new RequestQuote()
            {
                WholesalerId = 1,
                Beers = new List<RequestQuoteBeers>()
                {
                    new RequestQuoteBeers()
                    {
                        BeerId = 25,
                        Count = beerRequested
                    },
                }
            };

            var resultQuote = await service.GetQuote(quote);


            Assert.AreEqual((beerPrice * beerRequested), resultQuote.Price);
        }

        [TestMethod]
        public async Task TestGetQuotePriceBetween10And20Beers()
        {
            var breweryContextMoq = new Mock<IBreweryContext>();
            decimal beerPrice = 0.59M;
            int beerRequested = 15;
            breweryContextMoq.Setup(bq => bq.GetWholesaler(1))
                .Returns(Task.FromResult(new Wholesaler()
                {
                    Id = 1,
                    Name = "TestWholeSaler",
                    WholesalerStocks = new List<WholesalerStock>()
                    {
                        new WholesalerStock()
                        {
                            WholesalerId = 1,
                            BeerId = 25,
                            Count = 200,
                            Beer = new Beer()
                            {
                                Id = 25,
                                Name = "TestBeer",
                                AlcoolPercentage = 99.0M,
                                Price = beerPrice
                            }
                        }
                    }
                }));


            QuoteService service = new QuoteService(breweryContextMoq.Object);
            RequestQuote quote = new RequestQuote()
            {
                WholesalerId = 1,
                Beers = new List<RequestQuoteBeers>()
                {
                    new RequestQuoteBeers()
                    {
                        BeerId = 25,
                        Count = beerRequested
                    },
                }
            };

            var resultQuote = await service.GetQuote(quote);


            Assert.AreEqual((beerPrice * beerRequested * 0.9M), resultQuote.Price);
        }

        [TestMethod]
        public async Task TestGetQuotePriceExactly20ShouldReduce10Percent()
        {
            var breweryContextMoq = new Mock<IBreweryContext>();
            decimal beerPrice = 0.59M;
            int beerRequested = 20;
            breweryContextMoq.Setup(bq => bq.GetWholesaler(1))
                .Returns(Task.FromResult(new Wholesaler()
                {
                    Id = 1,
                    Name = "TestWholeSaler",
                    WholesalerStocks = new List<WholesalerStock>()
                    {
                        new WholesalerStock()
                        {
                            WholesalerId = 1,
                            BeerId = 25,
                            Count = 200,
                            Beer = new Beer()
                            {
                                Id = 25,
                                Name = "TestBeer",
                                AlcoolPercentage = 99.0M,
                                Price = beerPrice
                            }
                        }
                    }
                }));


            QuoteService service = new QuoteService(breweryContextMoq.Object);
            RequestQuote quote = new RequestQuote()
            {
                WholesalerId = 1,
                Beers = new List<RequestQuoteBeers>()
                {
                    new RequestQuoteBeers()
                    {
                        BeerId = 25,
                        Count = beerRequested
                    },
                }
            };

            var resultQuote = await service.GetQuote(quote);


            Assert.AreEqual((beerPrice * beerRequested * 0.9M), resultQuote.Price);
        }


        [TestMethod]
        public async Task TestGetQuotePriceMoreThan20Beers()
        {
            var breweryContextMoq = new Mock<IBreweryContext>();
            decimal beerPrice = 0.59M;
            int beerRequested = 199;
            breweryContextMoq.Setup(bq => bq.GetWholesaler(1))
                .Returns(Task.FromResult(new Wholesaler()
                {
                    Id = 1,
                    Name = "TestWholeSaler",
                    WholesalerStocks = new List<WholesalerStock>()
                    {
                        new WholesalerStock()
                        {
                            WholesalerId = 1,
                            BeerId = 25,
                            Count = 200,
                            Beer = new Beer()
                            {
                                Id = 25,
                                Name = "TestBeer",
                                AlcoolPercentage = 99.0M,
                                Price = beerPrice
                            }
                        }
                    }
                }));

            QuoteService service = new QuoteService(breweryContextMoq.Object);
            RequestQuote quote = new RequestQuote()
            {
                WholesalerId = 1,
                Beers = new List<RequestQuoteBeers>()
                {
                    new RequestQuoteBeers()
                    {
                        BeerId = 25,
                        Count = beerRequested
                    },
                }
            };

            var resultQuote = await service.GetQuote(quote);


            Assert.AreEqual((beerPrice * beerRequested * 0.8M), resultQuote.Price);
        }


        [TestMethod]
        public async Task TestGetQuotePriceMoreThan20BeersMultipleBeers()
        {
            var breweryContextMoq = new Mock<IBreweryContext>();
            decimal firstBeerPrice = 0.59M;
            decimal secondBeerPrice = 0.41M;
            int firstBeerCountRequest = 199;
            int secondBeerCountRequest = 56;
            int firstBeerId = 25;
            int secondBeerId = 36;


            breweryContextMoq.Setup(bq => bq.GetWholesaler(1))
                .Returns(Task.FromResult(new Wholesaler()
                {
                    Id = 1,
                    Name = "TestWholeSaler",
                    WholesalerStocks = new List<WholesalerStock>()
                    {
                        new WholesalerStock()
                        {
                            WholesalerId = 1,
                            BeerId = firstBeerId,
                            Count = 200,
                            Beer = new Beer()
                            {
                                Id = firstBeerId,
                                Name = "TestBeer",
                                AlcoolPercentage = 99.0M,
                                Price = firstBeerPrice
                            }
                        },
                        new WholesalerStock()
                        {
                            WholesalerId = 1,
                            BeerId = secondBeerId,
                            Count = 360,
                            Beer = new Beer()
                            {
                                Id = secondBeerId,
                                Name = "TestBeer2",
                                AlcoolPercentage = 99.0M,
                                Price = secondBeerPrice
                            }
                        },

                    }
                }));


            QuoteService service = new QuoteService(breweryContextMoq.Object);
            RequestQuote quote = new RequestQuote()
            {
                WholesalerId = 1,
                Beers = new List<RequestQuoteBeers>()
                {
                    new RequestQuoteBeers()
                    {
                        BeerId = firstBeerId,
                        Count = firstBeerCountRequest
                    },
                    new RequestQuoteBeers()
                    {
                        BeerId = secondBeerId,
                        Count = secondBeerCountRequest
                    }
                }
            };

            var resultQuote = await service.GetQuote(quote);


            Assert.AreEqual(((firstBeerPrice * firstBeerCountRequest) + (secondBeerPrice * secondBeerCountRequest)) * 0.8M, resultQuote.Price);
        }
    }
}
