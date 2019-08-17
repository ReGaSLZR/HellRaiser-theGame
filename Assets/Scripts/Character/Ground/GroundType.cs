namespace Character.Ground {

    /*
     * DOCUMENTATION:
     * Ground_Left and Ground_Right are serve as pair triggers.
     * Both have to detect ground as precondition. A tick is emitted when one of the pair exits ground.
     * 
     * Wall_Minus (left or down) and Wall_Plus (right or up) are NOT pair triggers.
     * One can work by itself. A tick is emitted whenever any of the wall detectors meets ground.
    */
    public enum GroundType
    {
        Ground_Center,
        Ground_Left,
        Ground_Right,
        
        Wall_Minus,
        Wall_Plus
    }
}