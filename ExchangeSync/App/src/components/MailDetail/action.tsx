export const ActionMenus = [
    {
        key: 'markReaded',
        iconProps:{
            iconName:"Mail"
        },
        name: '标记为未读',
    },
    {
        key: 'mark',
        iconProps:{
            iconName:"Calendar"
        },
        name: '标记',
    },{
        key:"answer",
        iconProps:{
            iconName:"Calendar"
        },
        name:"答复",
    },{
        key:"answer_all",
        iconProps:{
            iconName:"Calendar"
        },
        name:"全部答复",
    },{
        key:"to",
        iconProps:{
            iconName:"Calendar"
        },
        name:"转发",
    }
];

export const Styles = {
    persona: {
        root: {
            float: "left",
            marginLeft: 10
        }
    },
    time: {
        display: "inline-block",
        float: "right",
        marginRight: 10
    } as React.CSSProperties,
    overflowItem: {
        root: {
            padding:0
        }
    }
};