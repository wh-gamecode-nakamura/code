
//$p 定数
// レイヤー
public class Layer {
    public const float background = -1;
    public const float field = -2;
    public const float piece = -3;
    public const float mist = -4;
    public const float mark = -5;
    public const float UI = -6;
};

public class Field_color {
    public const int basic = 1;
    public const int can_move = 2;
    public const int attack_move = 3;
    public const int noMove = 4;
}
public class Coord {
    public const int o = 1; // origin
    public const int x = 3;
    public const int y = 5;

    // 盤面の長さが奇数でも偶数でも霧生成の処理を変えないための定数
    public const int mistRow = (((Coord.y + (Coord.y % 2)) / 2) - 1);
}

public class Mist {
    public const int no_mist = 0;
    public const int ally_mist = 1;
    public const int opp_mist = 2;
}

public class Because {
    public const int none = 0;
    public const int take_king = 1;
    public const int arrival_king = 2;
    public const int surrender = 3;
    public const int time_out = 4;
    public const int disconnect = 5;
}

public class Phase {
    public const int while_game = 1;
    public const int result_game = 2;
    public const int end_game = 3;
}

public class Line_type {
    public const int h = 1; // 縦
    public const int w = 2; // 横
}


//$p その他
////public const int P_NUMS = 2;

