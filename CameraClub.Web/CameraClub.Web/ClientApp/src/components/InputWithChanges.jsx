import React, { Component } from 'react';

export class InputWithChanges extends Component {
    handleChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;

        this.props.onChangeInput({ [name]: value });
    }

    render() {
        return (
            <>
                <input className="form-control" type={this.props.inputType} name={this.props.name} placeholder={this.props.placeholderText} value={this.props.value} onChange={(e) => { this.handleChange(e); }} />
            </>
        );
    }
}