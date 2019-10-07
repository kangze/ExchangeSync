const noOp = function () { };
export const ActionMenus = [
    {
        key: 'markReaded',
        iconProps: {
            iconName: "Mail"
        },
        name: '标记为未读',
        onClick: (mailId: string) => {
            alert("已经标记为未读!");
        }
    },
    {
        key: 'mark',
        iconProps: {
            iconName: "Mail"
        },
        name: '标记',
        onClick: (mailId: string) => {
            alert("已经标记!");
        }
    }, {
        key: "answer",
        iconProps: {
            iconName: "Mail"
        },
        name: "答复",
        onClick: (mailId: string) => {
            alert("已经答复!");
        }
    }, {
        key: "answer_all",
        iconProps: {
            iconName: "Mail"
        },
        name: "全部答复",
        onClick: noOp
    }, {
        key: "to",
        iconProps: {
            iconName: "Mail"
        },
        name: "转发",
        onClick: (mailId: string) => {
            alert("已经转发!");
        }
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
            padding: 0
        }
    }
};

