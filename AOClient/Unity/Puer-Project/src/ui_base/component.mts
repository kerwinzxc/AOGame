import "csharp";
import "puerts";
import UIElement from "./uielement.mjs";
import ptypeof = puer.$typeof;
import ppromise = puer.$promise;
import fgui = CS.FairyGUI;
import ET = CS.ET;
import AO = CS.AO;
import AOGame = CS.AO.AOGame;

export default class Component {

    public ui:UIElement;

    constructor(ui:UIElement) {
        this.ui = ui;
    }
}
