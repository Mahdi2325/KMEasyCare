var data=

{
    "timeline":
    {
        "headline": "Johnny B Goode",
        "type": "default",
        "startDate": "2009,1",
        "text": "<i><span class='c1'>���Ӌ��</span> <span class='c2'></span></i>",
        "asset":
        {
            "media": "assets/img/notes.png",
            "credit": "<a href='http://dribbble.com/shots/221641-iOS-Icon'>iOS Icon by Asher</a>",
            "caption": ""
        },
        "date": [
            {
                "startDate": "2009,2",
                "headline": "�o�����oӋ��",
                "text": "<div class='timeline-group-div'><label class='timeline-task-text'>�\�����}���o���ʩ��</label> <label style='float:right;color:#5e89af'>  IADL:14��;MMSE:15��;ADL:30��;GDS:20��;</label>  </div><div class='hack timeline-hack-border'></div> <div style='width:100%' class='timeline-group-div'>  <div class='timeline-group-div timeline-group-header' >�ű��׃</div><div class='timeline-group-content'><div class='timeline-group-content-lable'> <div> <div class='timeline-group-div'>  ᘌ���Ӗ��֮���������vҎ�t���c���ű�Ӗ��  </div>  </div> </div>  <div class='timeline-group-content-lable'> <div>  <div class='timeline-group-div'>  �̼��ֱ��ų������vҎ�t���c���ű�Ӗ��  </div>  </div>   </div> </div> </div>"
            },
            {
                "startDate": "2009,5",
                "headline": "Redesign of my portfolio",
                "text": "",
                "asset":
                {
                    "media": "",
                    "credit": "",
                    "caption": ""
                }
            },
            {
                "startDate": "2009,7",
                "headline": "Another time-lapse experiment",
                "text": "",
                "asset":
                {
                    "media": "http://vimeo.com/23237102",
                    "credit": "",
                    "caption": ""
                }
            },
            {
                "startDate": "2009,10",
                "headline": "Developed a Gmail Client",
                "text": "",
                "asset":
                {
                    "media": "http://dribbble.com/system/users/2559/screenshots/120088/shot_1298590416.jpg?1309796199",
                    "credit": "<a href='http://dribbble.com/shots/120088-Gmail-Pokki-Final-Ui'>by Justalab</a>",
                    "caption": ""
                }
            },
            {
                "startDate": "2010,3",
                "headline": "Logo Design for a pet shop",
                "text": "",
                "asset":
                {
                    "media": "http://dribbble.com/system/users/58661/screenshots/444003/pet___you.png?1330172683",
                    "credit": "<a href='http://dribbble.com/shots/444003-Pet-You'>by Nikita Lebedev</a>",
                    "caption": ""
                }
            },
            {
                "startDate": "2010,4",
                "headline": "Developed an iPad newspaper application",
                "text": "It was a challenge to create both the design and code in a week.",
                "asset":
                {
                    "media": "http://dribbble.com/system/users/14521/screenshots/228267/proto_v4_decoupe_2.png?1324546898",
                    "credit": "<a href='http://dribbble.com/shots/228267-Swiss-newspaper-reader-app-for-iPad-UI-UX-iOS'>by Jonathan Moreira</a>",
                    "caption": ""
                }
            },
            {
                "startDate": "2010,8",
                "headline": "Illustration for a big client.",
                "text": "",
                "asset":
                {
                    "media": "http://dribbble.com/system/users/13307/screenshots/366400/chameleon.jpg?1328028363",
                    "credit": "<a href='http://dribbble.com/shots/366400-Chameleon'>Chameleon by Mike</a>",
                    "caption": ""
                }
            },
            {
                "startDate": "2010,12",
                "headline": "Advert for Volkswagen",
                "text": "Created the website for their new advertising campaign.",
                "asset":
                {
                    "media": "http://www.youtube.com/watch?v=0-9EYFJ4Clo",
                    "credit": "",
                    "caption": ""
                }
            }
        ]
    }
}

$(function () {
	
	var timeline = new VMM.Timeline();
	//timeline.init(data);
	timeline.init("/Scripts/timeline/data.txt");
});
