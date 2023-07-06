using System.Collections;
using System.Collections.Generic;

class Hohei : Chara_sc
{
    public Hohei() {
        chara_name = "歩兵";
        pos_var_li = place_range(mine, null, 2);
        cost += 3.5f; // 6.5   2 + ((5-2)/2)
        promote_id = 101;
    }
}

class juuji : Chara_sc
{
    public juuji() {
        chara_name = "十字";
        pos_var_li = place_range(mine, null, 0);
        cost += 5.5f; // 8.5
    }
}
