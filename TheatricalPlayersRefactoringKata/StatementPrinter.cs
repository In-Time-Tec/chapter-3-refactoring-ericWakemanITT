using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            int totalAmount = 0;
            int volumeCredits = 0;
            string result = string.Format("Statement for {0}\n", invoice.Customer);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach(var perf in invoice.Performances)
            {
                Play play = plays[perf.PlayID];
                if (play.Type.ToLower() == "tragedy" || play.Type.ToLower() == "comedy")
                {
                    int currentAmount = calculateCurrentAmount(play, perf);
                    volumeCredits += calculateVolumeCredits(play, perf);

                    // print line for this order
                    result += String.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(currentAmount / 100), perf.Audience);
                    totalAmount += currentAmount;
                }
                else
                {
                    throw new Exception("unknown type: " + play.Type);
                }
            }
            result += String.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += String.Format("You earned {0} credits\n", volumeCredits);
            return result;
        }
    
        public int calculateVolumeCredits (Play play, Performance performance)
        {
            int volumeCredits = 0;
             // add volume credits
            volumeCredits += Math.Max(performance.Audience - 30, 0);
            // add extra credit for every ten comedy attendees
            if ("comedy" == play.Type.ToLower()) volumeCredits += (int)Math.Floor((decimal)performance.Audience / 5);
            return volumeCredits;
        }

        public int calculateCurrentAmount(Play play, Performance performance)
        {
            int thisAmount = 0;
            if(play.Type.ToLower() == "comedy")
            {
                thisAmount = 30000 + (300 * performance.Audience);
                 if (performance.Audience > 20) 
                  {
                    thisAmount += 10000 + 500 * (performance.Audience - 20);
                  }
            }
            else if(play.Type.ToLower() == "tragedy")
            {
                 thisAmount = 40000;
                if (performance.Audience > 30) 
                {
                    thisAmount += 1000 * (performance.Audience - 30);
                }
            }
            return thisAmount;
        }
    }
}
