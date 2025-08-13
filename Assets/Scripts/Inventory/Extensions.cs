using System.Text;
public static class Extensions
{
    public static string ToTooltipText(this ItemStatus status)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"<b>{status.Data.itemName}</b>");
        sb.AppendLine(status.Data.description);

        if (status.Data.itemType == ItemType.Consumable)
        {
            ConsumableStatus cStat = status as ConsumableStatus;
            foreach (ConsumeValue consumeValue in cStat.Data.consumes)
            {
                sb.Append($"<color=green>{consumeValue.value}</color>");
                switch (consumeValue.type)
                {
                    case ConsumeType.Health:
                        sb.Append("HP");
                        break;
                }
                sb.AppendLine();
            }
        }

        return sb.ToString();
    }
}
