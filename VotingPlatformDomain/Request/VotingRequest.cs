using System;
using System.Collections.Generic;
using System.Text;

namespace VotingPlatformDomain.Request
{
    public class VotingRequest:BaseRequest
    {
        public int VotingID { get; set; }
        public string VotingName { get; set; }
        public string VotingDescription { get; set; }
        public string DueDateString { get; set; }
        public DateTime DueDate { get { return convertToDatetime(DueDateString); } }
        public int CategoryID { get; set; }
        
        private DateTime convertToDatetime(string plainText)
        {
            string[] inputArray = plainText.Split("-");

            return DateTime.Parse(inputArray[1] + "/" + inputArray[0] + "/" + inputArray[2]);
        }
    }
}
