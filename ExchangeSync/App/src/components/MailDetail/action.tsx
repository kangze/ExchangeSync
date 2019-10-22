const noOp = function () { };
import axios from "axios";
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
export const DraftMenu=[
    {
        key: 'markReaded',
        iconProps: {
            iconName: "Delete"
        },
        name: '删除此邮件',
        onClick: (mailId: string) => {
            axios.post("/mail/Delete?mailid=" + mailId).then(response => {
                if (response.data.success) {
                    alert("删除成功!");
                }
            }).catch(u => {
                alert("请稍后重新尝试");
            })
        }
    },
]

export const SentMenu=[
    {
        key: 'markReaded',
        iconProps: {
            iconName: "Delete"
        },
        name: '删除此邮件',
        onClick: (mailId: string) => {
            axios.post("/mail/Delete?mailid=" + mailId).then(response => {
                if (response.data.success) {
                    alert("删除成功!");
                }
            }).catch(u => {
                alert("请稍后重新尝试");
            })
        }
    },
]

export const InBoxMenu = [
    {
        key: 'markReaded',
        iconProps: {
            iconName: "Read"
        },
        name: '标记为未读',
        onClick: (mailId: string) => {
            axios.post("/mail/setunreade?mailid=" + mailId).then(response => {
                if (response.data.success) {
                    alert("已经标记为未读!");
                }
            }).catch(u => {
                alert("请稍后重新尝试");
            })
        }
    },
    {
        key: 'markReaded',
        iconProps: {
            iconName: "Delete"
        },
        name: '删除此邮件',
        onClick: (mailId: string) => {
            axios.post("/mail/Delete?mailid=" + mailId).then(response => {
                if (response.data.success) {
                    alert("删除成功!");
                }
            }).catch(u => {
                alert("请稍后重新尝试");
            })
        }
    },
];
