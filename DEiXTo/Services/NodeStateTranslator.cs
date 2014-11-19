using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Services
{
    public class NodeStateTranslator
    {
        public const string Checked = "checked";
        public const string CheckedImplied = "checked_implied";
        public const string CheckedSource = "checked_source";
        public const string Grayed = "grayed";
        public const string GrayedImplied = "grayed_implied";
        public const string Unchecked = "dont_care";

        public static string StateToString(NodeState value)
        {
            switch (value)
            {
                case NodeState.Checked:
                    return Checked;
                case NodeState.CheckedSource:
                    return CheckedSource;
                case NodeState.Grayed:
                    return Grayed;
                case NodeState.Unchecked:
                    return Unchecked;
                case NodeState.GrayedImplied:
                    return GrayedImplied;
                case NodeState.CheckedImplied:
                    return CheckedImplied;
                default:
                    return null;
            }
        }

        public static NodeState StringToState(string value)
        {
            switch (value)
            {
                case Checked:
                    return NodeState.Checked;
                case CheckedImplied:
                    return NodeState.CheckedImplied;
                case CheckedSource:
                    return NodeState.CheckedSource;
                case Grayed:
                    return NodeState.Grayed;
                case GrayedImplied:
                    return NodeState.GrayedImplied;
                case Unchecked:
                    return NodeState.Unchecked;
            }

            return NodeState.Undefined;
        }

        public static int StateToImageIndex(NodeState value)
        {
            int imageIndex = -1;

            switch (value)
            {
                case NodeState.Checked:
                    imageIndex = 0;
                    break;
                case NodeState.CheckedImplied:
                    imageIndex = 1;
                    break;
                case NodeState.CheckedSource:
                    imageIndex = 2;
                    break;
                case NodeState.Grayed:
                    imageIndex = 3;
                    break;
                case NodeState.GrayedImplied:
                    imageIndex = 4;
                    break;
                case NodeState.Unchecked:
                    imageIndex = 5;
                    break;
                default:
                    imageIndex = -1;
                    break;
            }

            return imageIndex;
        }

        public static int StringToImageIndex(string value)
        {
            int index = -1;

            switch (value)
            {
                case Checked:
                    index = 0;
                    break;
                case CheckedImplied:
                    index = 1;
                    break;
                case CheckedSource:
                    index = 2;
                    break;
                case Grayed:
                    index = 3;
                    break;
                case GrayedImplied:
                    index = 4;
                    break;
                case Unchecked:
                    index = 5;
                    break;
            }

            return index;
        }
    }
}
