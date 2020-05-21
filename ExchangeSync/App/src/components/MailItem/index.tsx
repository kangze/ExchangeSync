import * as React from 'react';
import { IPersonaSharedProps, Persona, PersonaInitialsColor, PersonaSize } from 'office-ui-fabric-react/lib/Persona';
import { Stack } from 'office-ui-fabric-react/lib/Stack';
import { Text } from 'office-ui-fabric-react/lib/Text';
import { IconButton } from 'office-ui-fabric-react/lib/Button';
import { Spinner } from 'office-ui-fabric-react/lib/Spinner';
import { Shimmer, ShimmerElementsGroup, ShimmerElementType } from 'office-ui-fabric-react/lib/Shimmer';
import axios from "axios";
import { Depths } from '@uifabric/fluent-theme/lib/fluent/FluentDepths';
import { Icon } from 'office-ui-fabric-react/lib/Icon';
import Empty from "../_shared/Empty";
import { Nav, INavStyles, INavLinkGroup } from 'office-ui-fabric-react/lib/Nav';

import MailDetail from "../MailDetail";

const examplePersona: IPersonaSharedProps = {
    imageInitials: "M",
    size: PersonaSize.size48,
};

const colors = [
    "#750b1c",
    "#a4262c",
    "#498205",
    "#038387",
    "#005b70",
    "#0078d4",
    "#004e8c",
    "#8764b8",
    "#881798",
    "#8e562e",
]

const styles = {
    root: {
        marginTop: 10,
        marginLeft: 10,
        float: "left",
    },
    details: {
        //backgroundColor:"red",
        //marginTop: -25
    }
}

export interface IMailItemGroupedProps {
    items: IMailItemGroupProps[]
}

export interface IMailItemGroupProps {
    groupTitle: string,
    items: IMailItemProps[]
}

export interface IMailItemProps {
    /**
     * 发件人
     */
    sender: IMailContact,

    /**
     * 邮件标题
     */
    title: string,

    /**
     * 邮件的ID,用于查看详情
     */
    mailId: string,

    /**
     * 收件人的
     */
    recivers: IMailContact[],

    /**
     * 邮件简短的描述信息
     */
    description: string

    /**
     * 指示邮件是否被读取
     */
    readed?: boolean,

    /**
     * 邮件时间,是已经被服务器分号组的
     */
    date: string,

    /**
     * 附件信息
     */
    hasAttachments: boolean
}

export interface IMailContact {
    name: string,
    address: string
}

export default class MailItem extends React.PureComponent<any, any> {

    constructor(props: any) {
        super(props);
        if (props.staticContext && props.staticContext.data) {
            let data = props.staticContext.data;
            let user = props.staticContext.user;
            this.state = {
                groups: data as IMailItemGroupedProps[],
                user: user,
                mailId: "un"
            }
        } else if ((window as any).data) {
            let data = (window as any).data;
            delete (window as any).data;
            let user = (window as any).user;
            this.state = {
                groups: data as IMailItemGroupedProps[],
                user: user,
                mailId: "un"
            }
        } else {
            let user = (window as any).user;
            this.state = {
                loading: true,
                user: user,
                mailId: "un"
            }
        }
    }

    componentDidMount() {
        let self = this;
        if (this.props.type === "sended" && this.state.loading) {
            self.props.changeTitle("已发送邮件");
            axios.get("/mail/getasync?type=sended").then(response => {
                var data = response.data;
                self.setState({ groups: data, loading: false });
            }).catch(error => {
                alert('邮件服务器错误,请刷新页面再次尝试!');
            })
        } else if (this.props.type === "index" && this.state.loading) {
            self.props.changeTitle("收件箱");
            axios.get("/mail/getasync?type=index").then(response => {
                var data = response.data;
                self.setState({ groups: data, loading: false });
            }).catch(error => {
                alert('邮件服务器错误,请刷新页面再次尝试!');
            })
        } else if (this.props.type === "draft" && this.state.loading) {
            self.props.changeTitle("草稿箱");
            axios.get("/mail/getasync?type=draft").then(response => {
                var data = response.data;
                self.setState({ groups: data, loading: false });
            }).catch(error => {
                alert('邮件服务器错误,请刷新页面再次尝试!');
            })
        }
    }


    public handleClick(item: IMailItemProps, pc: boolean) {
        if (pc)
            this.setState({ mailId: encodeURIComponent(item.mailId) });
        else
            (this.props as any).history.push("/detail/" + encodeURIComponent(item.mailId));
    }

    public handle_nav_clikc() {
        //alert('1');
    }

    handleCreate() {
        (this.props as any).history.push("/create");
    }

    public renderItem(items: IMailItemProps[], pc: boolean) {
        return items.map(item => {
            let style = undefined;
            let selectedColor = undefined;
            if (pc && encodeURIComponent(item.mailId) == this.state.mailId) {
                selectedColor = "#dcdee4"
            }
            //没有读取的邮件，所有字体设置为粗体
            if (!item.readed) style = { fontWeight: "bold" } as React.CSSProperties;
            return (
                <Stack style={{ marginTop: 5, marginBottom: 10, backgroundColor: selectedColor }}>
                    <Persona
                        onClick={this.handleClick.bind(this, item, pc)}
                        imageInitials={item.sender.name[0]}
                        initialsColor={parseInt((10 * Math.random()).toString()) as PersonaInitialsColor}
                        size={PersonaSize.size40}
                        styles={styles}>
                        <div>
                            <div>
                                <Text key={item.sender.name} style={style} variant="medium" nowrap block>{item.sender.name}</Text>
                            </div>

                            <div>
                                <Text key={item.title} style={style} variant="medium" nowrap block>
                                    {item.title}
                                    {item.hasAttachments ? <Icon styles={{ root: { position: "absolute", right: 50, top: 20, fontSize: 15, color: "#005bac" } }} iconName="Attach" className="ms-IconExample" /> : null}

                                </Text>

                            </div>
                            <div>
                                <Text key={item.description} style={style} variant="medium" nowrap block>{item.description}</Text>
                            </div>

                            <div style={{ position: "absolute", right: 25, top: -5 }}>
                                <Text key={item.date} variant="medium" style={{ color: "#3f8d57" }} nowrap block>{item.date}</Text>
                            </div>
                        </div>
                    </Persona>
                    <div style={{ clear: "both" }}></div>
                    <hr style={{ backgroundColor: "#584b4b4f", marginTop: 16, height: 1, border: "none" }} />
                </Stack>
            );
        })
    }

    public render_addButton() {
        return (
            <div style={{ position: "fixed", borderRadius: 42, backgroundColor: "#005bac", height: 49, width: 54, right: 20, bottom: 20, paddingLeft: 10, paddingTop: 15, boxShadow: Depths.depth64 }} className="ms-hiddenMdUp">
                <IconButton
                    className="btnhover"
                    iconProps={{
                        iconName: 'Edit', styles: {
                            root: {
                                color: "white",
                                fontSize: 32,
                                left: 10
                            },
                        }
                    }}
                    title="Add"
                    ariaLabel="Add"
                    onClick={this.handleCreate.bind(this)}
                />
            </div>
        );
    }

    public render_itemList(pc: boolean) {
        return (
            (this.state as any).groups.map((group: any) => {
                return (
                    <div>
                        <div style={{ marginLeft: 15, marginTop: 10 }}>
                            <Text key={"group1"} variant="medium" nowrap block>{group.groupTitle}</Text>
                        </div>
                        {this.renderItem(group.items as any, pc)}
                    </div>
                );
            })
        );
    }

    public render(): JSX.Element {
        let wechat = this.state.user.wechat;
        let name = this.state.user.name;
        if (this.state.loading) {
            return (
                <div style={{ marginLeft: 15, marginTop: 10 }}>
                    <Shimmer customElementsGroup={this._getCustomElementsExampleTwo()} styles={{ root: { marginTop: 20, width: "90%" } }} />
                    <Shimmer customElementsGroup={this._getCustomElementsExampleTwo()} styles={{ root: { marginTop: 20, width: "85%" } }} />
                    <Shimmer customElementsGroup={this._getCustomElementsExampleTwo()} styles={{ root: { marginTop: 20, width: "90%" } }} />
                    <Shimmer customElementsGroup={this._getCustomElementsExampleTwo()} styles={{ root: { marginTop: 20, width: "75%" } }} />
                    <Shimmer customElementsGroup={this._getCustomElementsExampleTwo()} styles={{ root: { marginTop: 20, width: "80%" } }} />
                </div>
            );
        }
        let empty = false;
        if ((this.state as any).groups.length == 0)
            empty = true;

        // if ((this.state as any).groups.length == 0)
        //     return <Empty />

        // if (this.state.user.tt)
        //     return (
        //         <div>
        //             <div style={{ float: "left", width: "30%" }}>
        //                 <Stack tokens={{ childrenGap: 10 }}>
        //                     {this.render_itemList()}
        //                     {!wechat ?
        //                         this.render_addButton()
        //                         : undefined}
        //                 </Stack>
        //             </div>
        //             <div style={{ float: "left", width: "70%" }}>
        //                 <MailDetail mailId={this.state.mailId} />
        //             </div>
        //         </div>

        //     );
        // return (
        //     <Stack tokens={{ childrenGap: 10 }}>
        //         {this.render_itemList()}
        //         {!wechat ?
        //             this.render_addButton()
        //             : undefined}
        //     </Stack>
        // );

        return (
            <div>
                {/* 展示移动端的页面 */}
                <div className="ms-hiddenMdUp">
                    {empty ? <Empty /> :
                        <Stack tokens={{ childrenGap: 10 }}>
                            {this.render_itemList(false)}
                            {!wechat ?
                                this.render_addButton()
                                : undefined}
                        </Stack>
                    }

                </div>

                {/* 展示pc端的页面 */}
                <div className="ms-hiddenSm">
                    <div>
                        <div style={{ position: "absolute", left: 0, top: 48, bottom: 0, width: "10%" }}>
                            <Nav
                                styles={{
                                    root: {
                                        //backgroundColor: "red"
                                    }
                                }}
                                onLinkClick={this.handle_nav_clikc.bind(this)}
                                groups={[
                                    {
                                        name: name,
                                        links: [
                                            {
                                                key: "recived",
                                                name: "收件箱",
                                                url: "/",
                                            },
                                            {
                                                key: "sended",
                                                name: "已经发送",
                                                url: "/sended",
                                            },
                                            {
                                                key: "draft",
                                                name: "草稿",
                                                url: "/draft",
                                            },
                                            {
                                                key: "calendar",
                                                name: "日历",
                                                url: "/calendar",
                                            },
                                        ]
                                    }
                                ]}
                            />
                        </div>
                        {empty ? <Empty /> :
                            <div style={{ position: "absolute", left: "10%", top: 48, bottom: 0, width: "20%", borderLeftStyle: "solid", borderLeftWidth: 1, overflowY: "auto", borderLeftColor: "#dce1de" }}>
                                <Stack tokens={{ childrenGap: 10 }}>
                                    {this.render_itemList(true)}
                                    {!wechat ?
                                        this.render_addButton()
                                        : undefined}
                                </Stack>
                            </div>
                        }
                        <div style={{ position: "absolute", left: "30%", top: 48, bottom: 0, width: "70%", borderLeftStyle: "solid", borderLeftWidth: 1, borderLeftColor: "#dce1de" }}>
                            <MailDetail mailId={this.state.mailId} {...this.props} />
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    private _getCustomElementsExampleTwo = (): JSX.Element => {
        return (
            <div
                // tslint:disable-next-line:jsx-ban-props
                style={{ display: 'flex' }}
            >
                <ShimmerElementsGroup
                    shimmerElements={[{ type: ShimmerElementType.circle, height: 45 }, { type: ShimmerElementType.gap, width: 10, height: 60 }]}
                />
                <ShimmerElementsGroup
                    flexWrap={true}
                    shimmerElements={[
                        { type: ShimmerElementType.line, width: 400, height: 15 },
                        { type: ShimmerElementType.gap, width: 100, height: 30 },
                        { type: ShimmerElementType.line, width: 500, height: 15 }
                    ]}
                />
            </div>
        );
    };
}