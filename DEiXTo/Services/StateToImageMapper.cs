namespace DEiXTo.Services
{
    public class StateToImageMapper
    {
        public int GetImageFromState(NodeState state)
        {
            int imageIndex = -1;

            switch (state)
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
    }
}
