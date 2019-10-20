import * as React from "react";
import { BrowserRouter as Router, Route } from 'react-router-dom';

import Layout from "./Layout";
import Item from "./components/MailItem";
import MailDetail from "./components/MailDetail";
import MailCreate from "./components/MailCreate";
import Header from "./components/Header";
import MailCreateHeader from "./components/Header/CreateMail";

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

        //创建一个对象来保持内容
        let contentStorage = {
            content: null as any,
            setContent: function (content: any) {
                this.content = content;
            }
        }

        return (
            <Layout>
                <Route exact path="/" render={(param: any) => <Header title={this.state.title} {...param} />} />
                <Route exact path="/" render={(param: any) => <Item changeTitle={this.changeTitle.bind(this)} type="index" {...param} />} />
                <Route exact path="/sended" render={(param: any) => <Item changeTitle={this.changeTitle.bind(this)} type="sended" {...param} />} />
                <Route exact path="/draft" render={(param: any) => <Item changeTitle={this.changeTitle.bind(this)} type="draft" {...param} />} />

                <Route path="/detail/:mailId" render={(param: any) => <MailDetail changeTitle={this.changeTitle.bind(this)} {...param} />} />

                <Route path="/create" render={(param: any) => <MailCreateHeader changeTitle={this.changeTitle.bind(this)} {...param} />} />
                <Route path="/create" render={(param: any) => <MailCreate changeTitle={this.changeTitle.bind(this)} {...param} />} />

                <Route path="/reply/:mailId" render={(param: any) => <MailCreate changeTitle={this.changeTitle.bind(this)} {...param} />} />
            </Layout>
        );
    }
}