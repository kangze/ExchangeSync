import * as React from 'react';

import { Text } from 'office-ui-fabric-react/lib/Text';
import { TextField } from 'office-ui-fabric-react/lib/TextField';
import { Stack, IStackProps, IStackTokens } from 'office-ui-fabric-react/lib/Stack';
import axios from "axios";
import { PrimaryButton, Label } from 'office-ui-fabric-react';
import { Checkbox, ICheckboxProps } from 'office-ui-fabric-react/lib/Checkbox';
import { DatePicker, DayOfWeek, IDatePickerStrings } from 'office-ui-fabric-react';
import { TagPicker, IBasePicker, ITag } from 'office-ui-fabric-react/lib/Pickers';
import { Icon } from 'office-ui-fabric-react/lib/Icon';
import { Dropdown, DropdownMenuItemType, IDropdownStyles, IDropdownOption } from 'office-ui-fabric-react/lib/Dropdown';

const DayPickerStrings: IDatePickerStrings = {
    months: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],

    shortMonths: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],

    days: ['周日', '周一', '周二', '周三', '周四', '周五', '周六'],

    shortDays: ['日', '一', '二', '三', '四', '五', '六'],

    goToToday: 'Go to today',
    prevMonthAriaLabel: 'Go to previous month',
    nextMonthAriaLabel: 'Go to next month',
    prevYearAriaLabel: 'Go to previous year',
    nextYearAriaLabel: 'Go to next year',
    closeButtonAriaLabel: 'Close date picker'
};
const options: IDropdownOption[] = [
    { key: 'AM', text: '上午', itemType: DropdownMenuItemType.Header },
    { key: '00:00', text: '00:00' },
    { key: '01:00', text: '01:00' },
    { key: '02:00', text: '02:00', },
    { key: '03:00', text: '03:00' },
    { key: '04:00', text: '04:00' },
    { key: '05:00', text: '05:00', },
    { key: '06:00', text: '06:00' },
    { key: '07:00', text: '07:00' },
    { key: '08:00', text: '08:00', },
    { key: '09:00', text: '09:00' },
    { key: '10:00', text: '10:00' },
    { key: '11:00', text: '11:00', },
    { key: '12:00', text: '12:00' },
    { key: 'div', text: 'div', itemType: DropdownMenuItemType.Divider },
    { key: 'PM', text: '下午', itemType: DropdownMenuItemType.Header },
    { key: '13:00', text: '13:00' },
    { key: '14:00', text: '14:00' },
    { key: '15:00', text: '15:00', },
    { key: '16:00', text: '16:00' },
    { key: '17:00', text: '17:00' },
    { key: '18:00', text: '18:00', },
    { key: '19:00', text: '19:00' },
    { key: '20:00', text: '20:00' },
    { key: '21:00', text: '21:00', },
    { key: '22:00', text: '22:00' },
    { key: '23:00', text: '23:00' },
];
const dropdownStyles: Partial<IDropdownStyles> = {
    root: { width: "49%", marginRight: "1%", marginLeft: 0 }
};

declare var ZxEditor: any;

export default class CalendarCreate extends React.Component<any, any>{

    public constructor(props: any) {
        super(props);
        this.state = {
            firstDayOfWeek: DayOfWeek.Monday
        };
    }

    private _onFilterChanged = (filterText: string, tagList: ITag[]): ITag[] => {
        var promise = this._searchUser(filterText) as any;
        return promise;
    };

    private _getTextFromItem(item: ITag): string {
        return item.name;
    }

    private _searchUser(keyword: string): any {
        return new Promise(function (resolve, reject) {
            axios.get("/user/" + keyword).then(function (response) {
                var data = response.data;
                resolve(data);
            })
        })
    }

    componentDidMount() {
        let self = this;
        delete (window as any).content;
        var zxEditor = new ZxEditor('#e', {
            fixed: true,
            placeholder: "点击编辑..."
        });
        zxEditor.on('change', function () {
            var content = self.state.zxEditor.getContent();
            self.setState({ content });
        })
        self.setState({ zxEditor });
    }

    private _formatDate(date?: Date): string {
        if (!date)
            return "还未选择时间格式";
        return date.toLocaleDateString();
    }

    private handleSelectDate(name?: string, date?: Date) {
        this.setState({ [name]: date });
    }

    private _handleInputChange(name: string, e: any) {
        if (name == "AddToSkype" || name == "FullDay") {
            this.setState({
                [name]: !this.state[name]
            })
            return;
        }
        this.setState({
            [name]: e.target.value
        })
    }

    private _handleChange(name: string, items: ITag[]) {
        this.setState({ [name]: items });
    }

    private dropdownChange(name: string, e: any, option?: IDropdownOption, index?: number) {
        this.setState({ [name]: option.key });
        console.log(option);
    }

    componentDidUpdate() {
        (window as any).createCalendar = this.state;
    }

    public render() {
        const { firstDayOfWeek } = this.state;
        const stackTokens: IStackTokens = { childrenGap: 30 };
        return (
            <Stack tokens={{ childrenGap: 20 }} styles={{ root: { width: "100%" } }}>
                <br />
                <TextField iconProps={{
                    iconName: 'LocationDot', styles: {
                        root: {
                            fontSize: 16,
                        }
                    }
                }} value={this.state.title} onChange={this._handleInputChange.bind(this, "title")} label="主题：" underlined />
                <TextField iconProps={{
                    iconName: 'Location', styles: {
                        root: {
                            fontSize: 16,
                        }
                    }
                }} value={this.state.location} onChange={this._handleInputChange.bind(this, "location")} label="地点：" underlined />
                <Checkbox styles={{ root: { marginLeft: 10 } }} label="在线会议(Skype)" checked={this.state.AddToSkype} onChange={this._handleInputChange.bind(this, "AddToSkype")} />
                <Checkbox styles={{ root: { marginLeft: 10 } }} label="全天事件" checked={this.state.FullDay} onChange={this._handleInputChange.bind(this, "FullDay")} />

                <Stack horizontal tokens={stackTokens} verticalAlign="start">
                    <DatePicker style={{ width: "49%", marginLeft: "1%" }} value={this.state.start} onSelectDate={this.handleSelectDate.bind(this, "start")} formatDate={this._formatDate} firstDayOfWeek={firstDayOfWeek} strings={DayPickerStrings} placeholder="选择日期..." ariaLabel="选择日期..." label="起始时间：" />
                    {
                        this.state["FullDay"] ? undefined :
                            <Dropdown onChange={this.dropdownChange.bind(this, "startTime")} selectedKey={this.state.startTime} placeholder="请选择时间点" label="时间点：" options={options} styles={dropdownStyles} />
                    }
                </Stack>
                {
                    this.state["FullDay"] ? undefined :
                        <Stack horizontal tokens={stackTokens} verticalAlign="start">
                            <DatePicker style={{ width: "49%", marginLeft: "1%" }} value={this.state.start} onSelectDate={this.handleSelectDate.bind(this, "end")} formatDate={this._formatDate} firstDayOfWeek={firstDayOfWeek} strings={DayPickerStrings} placeholder="选择日期..." ariaLabel="选择日期..." label="结束时间：" />
                            <Dropdown onChange={this.dropdownChange.bind(this, "endTime")} selectedKey={this.state.endTime} placeholder="请选择时间点" label="时间点：" options={options} styles={dropdownStyles} />
                        </Stack>
                }


                <TextField id="attachmentCanlendar" multiple label="附件:" underlined type="file" />
                <Label><Icon iconName="ConnectContacts" styles={{ root: { fontSize: 25 } }} /><Text variant="medium" style={{ fontWeight: 700 }}>参会人员:</Text></Label>
                <TagPicker
                    styles={{ root: { marginTop: "0px !important" }, text: { borderTopStyle: "none", borderRightStyle: "none", borderLeftStyle: "none" } }}
                    onResolveSuggestions={this._onFilterChanged}
                    getTextFromItem={this._getTextFromItem}
                    pickerSuggestionsProps={{
                        suggestionsHeaderText: '请输入姓名/邮件地址',
                        noResultsFoundText: '没有找到该用户'
                    }}
                    itemLimit={100}
                    onChange={this._handleChange.bind(this, "attendees")}
                />
                <Label>&nbsp;&nbsp;&nbsp;内容:</Label>
                <div id="e"></div>
            </Stack>
        );
    }
}