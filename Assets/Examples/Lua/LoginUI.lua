-------------控件定义--------------
-- !@#start
LoginUI = 
{
  --!@#definestart

  --!@#defineend
}


-------------输入监听--------------
function LoginUI:InitBtnListener()

end

-- !@#startClick

-- !@#endClick
-- !@#end

------------生命周期------------------
function LoginUI:OnLoadSuccess()
	self:InitBtnListener();
	self:InitFEventListener();
end

function LoginUI:OnInit()
  
end

function LoginUI:OnClickMaskArea()
  
end

function LoginUI:Dispose()
  
end

function LoginUI:OnClose()
  
end
