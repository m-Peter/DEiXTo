public enum NodeState
{
    Checked, // Match and Extract Content - REQUIRED
    CheckedSource, // Match and Extract Source - REQUIRED
    Grayed, // Match - REQUIRED
    Unchecked, // Don't care
    GrayedImplied, // Match - OPTIONAL
    CheckedImplied, // Match and Extract Content - OPTIONAL
    Undefined
}