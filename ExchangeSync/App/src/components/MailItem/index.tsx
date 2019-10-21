import * as React from 'react';
import { IPersonaSharedProps, Persona, PersonaInitialsColor, PersonaSize } from 'office-ui-fabric-react/lib/Persona';
import { Stack } from 'office-ui-fabric-react/lib/Stack';
import { Text } from 'office-ui-fabric-react/lib/Text';
import { IconButton } from 'office-ui-fabric-react/lib/Button';
import { Spinner } from 'office-ui-fabric-react/lib/Spinner';
import { Shimmer, ShimmerElementsGroup, ShimmerElementType } from 'office-ui-fabric-react/lib/Shimmer';
import axios from "axios";
import { Icon } from 'office-ui-fabric-react/lib/Icon';


const examplePersona: IPersonaSharedProps = {
    imageInitials: "M",
    size: PersonaSize.size48,
};

const styles = {
    root: {
        marginTop: 10,
        marginLeft: 10,
        float: "left"
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
            this.state = {
                groups: data as IMailItemGroupedProps[]
            }
        } else if ((window as any).data) {
            let data = (window as any).data;
            delete (window as any).data;
            this.state = {
                groups: data as IMailItemGroupedProps[]
            }
        } else {
            this.state = {
                loading: true
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
            })
        } else if (this.props.type === "index" && this.state.loading) {
            self.props.changeTitle("收件箱");
            axios.get("/mail/getasync?type=index").then(response => {
                var data = response.data;
                self.setState({ groups: data, loading: false });
            })
        } else if (this.props.type === "draft" && this.state.loading) {
            self.props.changeTitle("草稿箱");
            axios.get("/mail/getasync?type=draft").then(response => {
                var data = response.data;
                self.setState({ groups: data, loading: false });
            })
        }
    }


    public handleClick(item: IMailItemProps) {
        (this.props as any).history.push("/detail/" + encodeURIComponent(item.mailId));
    }

    handleCreate() {
        (this.props as any).history.push("/create");
    }

    public renderItem(items: IMailItemProps[]) {
        return items.map(item => {
            let style = undefined;
            //没有读取的邮件，所有字体设置为粗体
            if (!item.readed) style = { fontWeight: "bold" } as React.CSSProperties;
            return (
                <Stack>
                    <Persona
                        onClick={this.handleClick.bind(this, item)}
                        imageInitials={item.sender.name[0]}
                        initialsColor={PersonaInitialsColor.magenta}
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
                    <hr style={{ backgroundColor: "#927a7a", marginTop: 10, height: 1, border: "none" }} />
                </Stack>
            );
        })
    }

    public render(): JSX.Element {
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
        if ((this.state as any).groups.length == 0)
            return <div>服务器正确返回,但是没有内容哦...</div>
        return (
            <Stack tokens={{ childrenGap: 10 }}>
                {(this.state as any).groups.map((group: any) => {
                    return (
                        <div>
                            <div style={{ marginLeft: 15, marginTop: 10 }}>
                                <Text key={"group1"} variant="medium" nowrap block>{group.groupTitle}</Text>
                            </div>
                            {this.renderItem(group.items as any)}
                        </div>
                    );
                })}
                <div style={{ position: "fixed", right: 20, bottom: 20 }}>
                    <IconButton
                        iconProps={{
                            iconName: 'EditMail', styles: {
                                root: {
                                    color: "#0a61af",
                                    fontSize: 48,
                                },
                                imageContainer: {
                                    height: 128
                                }
                            }
                        }}
                        title="Add"
                        ariaLabel="Add"
                        onClick={this.handleCreate.bind(this)}
                    />
                </div>
            </Stack>
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