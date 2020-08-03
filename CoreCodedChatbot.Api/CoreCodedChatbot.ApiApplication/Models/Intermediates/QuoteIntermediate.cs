using System;

namespace CoreCodedChatbot.ApiApplication.Models.Intermediates
{
    public class QuoteIntermediate
    {
        public int QuoteId { get; set; }
        public string QuoteText { get; set; }
        public string CreatedBy { get; set; }
        public bool Disabled { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedAt { get; set; }
    }
}