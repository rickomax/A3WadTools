#SYNONYM wand {TYPE WALL;}
/////////////////////////////////////////////////////////
// Debug panel - to display fps*10, nexus and error skill
/////////////////////////////////////////////////////////
SKILL   fps     { VAL 1; }
SKILL view_ang {}
///////////////////////////////////////////////
FONT    standard_font,<msgfont.pcx>,5,18;
#FONT    display_font,<dispfont.pcx>,14,28;
PANEL debug_panel {
	POS_X           0;
	POS_Y           4;
	DIGITS  20,20,3,standard_font,160,fps;   // frames per second * 10
	DIGITS  26,33,2,standard_font,1,ACTIVE_NEXUS;
        DIGITS  26,46,2,standard_font,1,ERROR;
#        DIGITS  26,59,2,standard_font,1,ACTIVE_OBJTICKS;
#        DIGITS  26,72,2,standard_font,1,ACTIONS;
#        DIGITS  70,20,5,standard_font,1,price;
#        DIGITS  70,33,5,standard_font,1,id;
#        DIGITS  70,46,4,standard_font,100,ghost.ANGLE;
#        DIGITS  70,59,4,standard_font,100,view_ang;
        FLAGS   REFRESH;
}
///////////////////////////////////////////////
ACTION update_dbpanel {
	RULE    fps = 0.9*fps + 0.1*TIME_FAC;
	RULE view_ang=ABS(ghost.ANGLE-SHOOT_ANGLE);
}

ACTION set_debug {
	SET     EACH_TICK.10,update_dbpanel;
	SET     PANELS.7,debug_panel;
}













































