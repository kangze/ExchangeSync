const noOp = function () { };
export const ActionMenus = [
    {
        key: 'markReaded',
        icon: "Mail",
        name: '标记为未读',
        onClick: noOp
    },
    {
        key: 'mark',
        icon: "Mail",
        name: '标记',
        onClick: noOp
    }, {
        key: "answer",
        icon: "Mail",
        name: "答复",
        onClick: noOp
    }, {
        key: "answer_all",
        icon: "Mail",
        name: "全部答复",
        onClick: noOp
    }, {
        key: "to",
        icon: "Mail",
        name: "转发",
        onClick: noOp
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

