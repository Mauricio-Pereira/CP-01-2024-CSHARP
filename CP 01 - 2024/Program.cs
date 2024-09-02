// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Web;
using CP_01___2024;
using HtmlAgilityPack;

var urlMain = "https://scryfall.com/sets/pblb";
var urls = new List<String>();
var cards = new List<SfCard>();
var html = new HtmlWeb().LoadFromWebAsync(urlMain).Result;
var cardsDiv = html.DocumentNode.SelectNodes("//*[@class='card-grid-inner']");


foreach (var node in cardsDiv)
{
    urls.AddRange(node.Descendants("a").Select(a => a.GetAttributeValue("href", String.Empty)));
}

Parallel.ForEach(urls, link =>
{
   var htmlCard = new HtmlWeb().LoadFromWebAsync(link).Result;
   var cardName = HttpUtility.HtmlDecode(htmlCard.DocumentNode.SelectSingleNode("//*[@class='card-text-card-name']")
       .InnerText.Trim());

   var cardDesc = htmlCard.DocumentNode.SelectSingleNode("//*[@class='card-text-oracle']");
   var paragraphs = cardDesc.SelectNodes(".//p");

   var cardDescText = string.Join(" ",
       paragraphs.Select(p => HttpUtility.HtmlDecode(p.InnerText.Trim()
           .Replace("\r\n", " ")
           .Replace("\r", " ")
           .Replace("\n", " "))));
   cards.Add(new SfCard(cardName, cardDescText));

   html = null;
   GC.Collect();
});


using (var context = new SfCardDbContext())
{
    context.Database.EnsureCreated();
    context.SFCARDS.AddRange(cards);
    context.SaveChanges();
    var cardsDb = context.SFCARDS.ToList();
    var csv = new StringBuilder();
    {
        csv.AppendLine("ID | NAME | DESCRIPTION");
    }
    foreach (var card in cardsDb)
    {
        csv.AppendLine($"{card.ID} | {card.NAME} | {card.DESCRIPTION}");
    }
    File.WriteAllText("cards.csv", csv.ToString(), Encoding.UTF8);

}
