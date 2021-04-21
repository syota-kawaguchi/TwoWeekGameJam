using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageText : MonoBehaviour
{
    public static List<string> messagesList = new List<string>
    {
        "testtesttesttesttesttesttesttesttesttesettsetsetsetsetsetestsetsetsetsetsetsetsetsetsetsetsetsetsetsetsetsetsetsetsetsetsetsetsetsetsettse"
    };

    public static string GetItemText(string itemName) {
        return itemName + "を手に入れた";
    }

    public static string checkBookShelfText() {
        return GameTrigger.isFallBookFromShelf ? "ドライバーの先端と組み合わせればドライバーとしてつかえそうだ。<>" + GetItemText("ドライバー") : "難しそうな本が並んでいる";
    }

    public static string checkDoorText(bool isOpen) { 
        return isOpen ? "カギがあいた" : "カギがかかっている";
    }

    public static string LockedByScrew() {
        return "ネジで閉まられていて開けられない";
    }

    public static string Check() {
        return "調べる";
    }

    public static string Open() {
        return "開ける";
    }

    public static string Close() {
        return "閉じる";
    }

    public static string PickUp() { 
        return "拾う"; 
    }

    public static string Hide() {
        return "隠れる";
    }

    public static string Exit() {
        return "出る";
    }
    public static string OpenDoor() {
        return "ドアが開いた";
    }

    public static string Locked() { 
        return "鍵がかかっている";
    }

    public static string ExchangePlayerItem(GameObject obj1, GameObject obj2) {
        return obj1.name + "と" + obj2 + "を入れ替えた";
    }

    public static string PutPlayerItem(GameObject obj) {
        return obj.name + "を置いた";
    }

    public static string CorridorDoorSwitchText() {
        return "ロックがかかっていてボタンが押せない。パスワードを入力しないといけないようだ。";
    }

    public static string DownLeverText() {
        return "レバーを下げますか？ はい / Y  :  いいえ / N";
    }

    public static string ShouldHide() {
        return GameTrigger.isEnemyRoomLocked? "どこかの鍵が空いた" : "やばい！！　かくれないと";
    }

    public static string Refrain() {
        return "やめておこう";
    }
    public static string ComeOffScrew() {
        return "ネジが外れた";
    }

    public static string somethingInEnvelop() {
        return "封筒の中になにか入っている。";
    }

    public static string somethingApproach() {
        return "何かが近づいてくる。隠れたほうがいいかも";
    }

    public static string GameOver() {
        return "敵に捕まってしまった。";
    }

    public static string RoomDText() {
        return "中から物音がする。さっきの化け物がいるかも知れない<> 南京錠があればこの扉にカギをかけられそうだ";
    }

    public static string RoomDLockText() {
        return "南京錠でドアに鍵をかけた";
    }

    public static string TryErase() {
        return "消しゴムで消えそうなところを消してみた。";
    }

    public static string lookErase() { 
       return "ところどころ消しゴムで消せそうだ";
    }

    public static string HearUnlockSoundSomeWhere() {
        return "どこかで鍵が開く音がした。";
    }

    public static string BoxIsEmpty() {
        return "箱の中は空っぽだ";
    }
}
