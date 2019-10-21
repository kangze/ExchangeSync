import * as React from 'react';

import { Text } from 'office-ui-fabric-react/lib/Text';
import axios from "axios";
import { IconButton } from 'office-ui-fabric-react/lib/Button';
import { Spinner } from 'office-ui-fabric-react/lib/Spinner';

export default class CalendarItem extends React.Component<any, any>{

    constructor(props: any) {
        super(props);
        if (props.staticContext && props.staticContext.data) {
            let data = props.staticContext.data;
            this.state = {
                groups: data,
                loading: false,
            }
        } else if ((window as any).data) {
            let data1 = (window as any).data;
            delete (window as any).data;
            this.state = {
                groups: data1,
                loading: false
            }
        } else {
            this.state = { loading: true };
        }
    }

    componentDidMount() {
        if (!this.state.loading)
            return;
        let self = this;
        let mailId = this.props.match.params.mailId;
        axios.get("/mail/GetMail?mailId=" + mailId).then(response => {
            let data = response.data;
            self.setState({
                loading: false,
                groups: data
            })
        })
    }

    private _handleCreateCalendar(e: any) {
        (this.props as any).history.push("/createCalendar");
    }

    private _reander_month(year: number, month: number, data: any) {
        let imgUrl = "/img/" + month + ".jpg";
        return (
            <div>
                <div style={{ width: "100%", height: 200, backgroundImage: 'url(' + imgUrl + ')', backgroundRepeat: "round" }}>
                    <div style={{ paddingLeft: "20%", paddingTop: 20 }}>
                        <Text variant="xLarge" style={{ color: "black" }}>{year}年{month}月</Text>
                    </div>
                </div>
                {data.map((u: any) => {
                    return (
                        <div>
                            <div style={{ float: "left", width: "20%", textAlign: "center", height: 45 }}>
                                <Text variant="medium" style={{ color: "black" }}>{u.week}</Text>
                                <br />
                                <Text variant="large" style={{ color: "black" }}>{u.day}</Text>
                            </div>
                            <div style={{ float: "left", width: "77%", backgroundColor: "#079848", borderRadius: 5, height: 45 }}>
                                <div style={{ paddingTop: 8, paddingLeft: 13 }}>
                                    <Text variant="large" style={{ color: "white" }}>{u.title}</Text>
                                </div>
                            </div>
                        </div>
                    );
                })}
            </div>
        );
    }

    public render() {
        let self = this;
        if (this.state.loading)
            return <Spinner styles={{ root: { marginTop: 40 } }} label="正在加载数据..." />
        return (
            <div>
                {this.state.groups.map((group: any) => {
                    return self._reander_month(group.key.year, group.key.month, group.data);
                })}
                <div style={{ position: "fixed", right: 20, bottom: 20 }}>
                    <IconButton
                        iconProps={{
                            iconName: 'CalendarSettings', styles: {
                                root: {
                                    color: "#0a61af",
                                    fontSize: 54,
                                }
                            }
                        }}
                        styles={{ root: { height: 64 } }}
                        title="Add"
                        ariaLabel="Add"
                        onClick={this._handleCreateCalendar.bind(this)}
                    />
                </div>
            </div>
        );
    }
}