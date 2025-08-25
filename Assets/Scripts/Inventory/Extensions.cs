using System.Text;

public static class Extensions
{
    public static string ToTooltipText(this ItemStatus status)
    {
        if (status == null || status.Data == null)
            return string.Empty;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"<b>{status.Data.itemName}</b>");
        sb.AppendLine(status.Data.description);

        if (status.Data.itemType == ItemType.Consumable)
        {
            ConsumableStatus cStat = status as ConsumableStatus;
            if (cStat != null && cStat.Data != null && cStat.Data.consumes != null)
            {
                foreach (ConsumeValue consumeValue in cStat.Data.consumes)
                {
                    sb.Append($"<color=green>{consumeValue.value}</color>");
                    switch (consumeValue.type)
                    {
                        case ConsumeType.Health:
                            sb.Append("HP");
                            break;
                            // 다른 타입도 여기 추가 가능
                    }
                    sb.AppendLine();
                }
            }
        }

        return sb.ToString();
    }
}