import * as React from "react";
import { BrowserRouter as Router, Route } from 'react-router-dom';
import Layout from "./Layout";
import Item from "./components/MailItem";
import MailDetail from "./components/MailDetail";
import MailCreate from "./components/MailCreate";
import Header from "./components/Header";
import MailCreateHeader from "./components/Header/CreateMail";
import MailCalendar from "./components/MailCalendar";
import MailCalendarCreate from "./components/MailCalendar/CalendarCreate";
import { relative } from "path";
import { IconButton } from 'office-ui-fabric-react/lib/Button';
import { Depths } from '@uifabric/fluent-theme/lib/fluent/FluentDepths';
import Editor from "./components/_shared/Editor";

declare function require(moduleName: string): any;
const AnimatedRoute = require('react-router-transition').AnimatedRoute;


export default class App extends React.Component<any, any> {

    constructor(props: any) {
        super(props);
        this.state = {
            title: "",
        }
    }

    public changeTitle(title: string) {
        this.setState({ title });
    }

    public render() {
        return (
            <Layout>
                <Route
                    atEnter={{ offset: -100 }}
                    atLeave={{ offset: 100 }}
                    atActive={{ offset: 0 }}
                    mapStyles={(styles: any) => ({
                        transform: `translateX(${styles.offset}%)`,
                        width: "100%",
                    })}
                    className="route-wrapper"
                    exact path="/" render={(param: any) => {
                        return (<div>
                            <Header title={"收件箱"} {...param} />
                            <Item changeTitle={this.changeTitle.bind(this)} type="index" {...param} />
                        </div>);
                    }} />

                <Route
                    atEnter={{ offset: 100 }}
                    atLeave={{ offset: 100 }}
                    atActive={{ offset: 0 }}
                    mapStyles={(styles: any) => ({
                        transform: `translateX(${styles.offset}%)`,
                        width: "100%",
                    })}
                    className="route-wrapper"
                    exact path="/create" render={(param: any) => {
                        return (<div>
                            <MailCreateHeader message={"发送成功"} title={"新建邮件"} {...param} />
                            <MailCreate changeTitle={this.changeTitle.bind(this)} {...param} />
                        </div>);
                    }} />


                <Route exact path="/sended" render={(param: any) => {
                    return (<div>
                        <Header title={"已发送邮件"} {...param} />
                        <Item changeTitle={this.changeTitle.bind(this)} type="sended" {...param} />
                    </div>);
                }} />

                <Route exact path="/draft" render={(param: any) => {
                    return (<div>
                        <Header title={"草稿箱"} {...param} />
                        <Item changeTitle={this.changeTitle.bind(this)} type="draft" {...param} />
                    </div>);
                }} />

                <Route exact path="/detail/:mailId" render={(param: any) => {
                    return (<div>
                        <Header title={"邮件详情"} {...param} />
                        <MailDetail changeTitle={this.changeTitle.bind(this)} {...param} />
                    </div>);
                }} />

                <Route exact path="/reply/:mailId" render={(param: any) => {
                    return (<div>
                        <MailCreateHeader message={"回复成功"} title={"回复"} {...param} />
                        <MailCreate changeTitle={this.changeTitle.bind(this)} {...param} />
                    </div>);
                }} />

                <Route exact path="/calendar" render={(param: any) => {
                    return (<div>
                        <Header message={"创建日历事件成功"} title={"我的日历/会议"} {...param} />
                        <MailCalendar showGoToToday={true} autoNavigateOnSelection={false} dateRangeType={0} isMonthPickerVisible={false} changeTitle={this.changeTitle.bind(this)} {...param} />
                    </div>);
                }} />

                <Route exact path="/createCalendar" render={(param: any) => {
                    return (<div>
                        <MailCreateHeader message={"创建会议成功"} title={"创建会议"} {...param} />
                        <MailCalendarCreate changeTitle={this.changeTitle.bind(this)} {...param} />
                    </div>);
                }} />
            </Layout>
        );
    }
}